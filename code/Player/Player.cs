using Mbk.Admin;
using Mbk.Admin.Logs;
using Mbk.RoleplayAPI.Entities.Weapons;
using Mbk.RoleplayAPI.Inventory;
using Mbk.RoleplayAPI.Models;
using Sandbox;
using Mbk.RoleplayAPI.Database;
using Mbk.RoleplayAPI.Database.DTO;
using Mbk.RoleplayAPI.UI.Shared.Chat;
using Mbk.RoleplayAPI.UI.RootPanels;
using Mbk.RoleplayAPI.Entities;

namespace Mbk.RoleplayAPI.Player;

/// <summary>
/// This is what you should derive your player from. This base exists in addon code
/// so we can take advantage of codegen for replication. The side effect is that we
/// can put stuff in here that we don't need to access from the engine - which gives
/// more transparency to our code.
/// </summary>
[Title( "RoleplayPlayer" ), Icon( "emoji_people" )]
public partial class RoleplayPlayer : AnimatedEntity
{
	/// <summary>
	/// The PlayerController takes player input and moves the player. This needs
	/// to match between client and server. The client moves the local player and
	/// then checks that when the server moves the player, everything is the same.
	/// This is called prediction. If it doesn't match the player resets everything
	/// to what the server did, that's a prediction error.
	/// You should really never manually set this on the client - it's replicated so
	/// that setting the class on the server will automatically network and set it
	/// on the client.
	/// </summary>
	[Net, Predicted]
	public PawnController Controller { get; set; }

	/// <summary>
	/// This is used for noclip mode
	/// </summary>
	[Net, Predicted]
	public PawnController DevController { get; set; }

	[Net, Predicted] public Entity ActiveChild { get; set; }
	[ClientInput] public Vector3 InputDirection { get; protected set; }
	[ClientInput] public Entity ActiveChildInput { get; set; }
	[ClientInput] public Angles ViewAngles { get; set; }
	public Angles OriginalViewAngles { get; private set; }

	/// <summary>
	/// Player's inventory for entities that can be carried. See <see cref="Mbk.RoleplayAPI.Player.BaseCarriable"/>.
	/// </summary>
	public CarriableHolster Holster { get; protected set; }

	public RoleplayPlayer()
	{
		Holster = new CarriableHolster( this );
		Data = new();

		if ( Game.IsServer )
		{
			CreateInventories();
		}
	}

	public void CreatePawn( IClient client )
	{
		Game.AssertServer();

		client.Pawn = this;
		_ = InitDatabase();

		Backpack.AddConnection( client );

		foreach ( var marker in Markers )
		{
			AddMapMarker( To.Single( client ), marker.Position, marker.Color );
		}
	}

	async Task InitDatabase()
	{
		Game.AssertServer();

		var instance = Database.Database.Get<PlayerTable>();

		if ( instance == null )
			return;

		if ( !instance.Exist( Client.SteamId ) )
		{
			Log.Info( "Player not exist, Creating ..." );

			await instance.Insert( new UserDTO()
			{
				SteamId = Client.SteamId,
				Name = Client.Name
			} );
		}

		var data = instance.Get( Client );
		Data.FirstName = data.FirstName;
		Data.LastName = data.LastName;
		Data.Nationality = data.Nationality;
		Data.Money = data.Money;
		Data.Bank = data.Bank;
		Data.Level = data.Level;
		Data.XP = data.XP;
		Data.JobId = data.JobId;
		Data.GradeId = data.GradeId;
		Data.Hunger = data.Hunger;
		Data.Thirst = data.Thirst;
		Data.HasBankCard = data.HasBankCard;
		Data.HasBankDetails = data.HasBankDetails;
		Data.IsNew = data.IsNew;
	}

	protected override void OnDestroy()
	{
		if ( Game.IsServer )
		{
			InventorySystem.Remove( Backpack );
		}

		base.OnDestroy();
	}

	/// <summary>
	/// Return the controller to use. Remember any logic you use here needs to match
	/// on both client and server. This is called as an accessor every tick.. so maybe
	/// avoid creating new classes here or you're gonna be making a ton of garbage!
	/// </summary>
	public virtual PawnController GetActiveController()
	{
		if ( DevController != null ) return DevController;

		return Controller;
	}

	/// <summary>
	/// Called every tick to simulate the player. This is called on the
	/// client as well as the server (for prediction). So be careful!
	/// </summary>
	public override void Simulate( IClient cl )
	{
		if ( LifeState == LifeState.Dead )
			return;

		if ( ActiveChildInput.IsValid() && ActiveChildInput.Owner == this )
		{
			ActiveChild = ActiveChildInput;
		}

		var controller = GetActiveController();
		if ( controller != null )
		{
			EnableSolidCollisions = !controller.HasTag( "noclip" );

			SimulateAnimation( controller );
			controller?.Simulate( cl, this );
		}

		if ( Input.Pressed( "Drop" ) )
		{
			var dropped = Holster.DropActive();
			Log.Info( dropped );
			if ( dropped != null )
			{
				dropped.PhysicsGroup.ApplyImpulse( Velocity + EyeRotation.Forward * 500.0f + Vector3.Up * 100.0f, true );
				dropped.PhysicsGroup.ApplyAngularImpulse( Vector3.Random * 100.0f, true );

				TimeSinceDropped = 0;
			}
		}

		if ( Input.Pressed( "View" ) )
		{
			ThirdPerson = !ThirdPerson;
		}

		if ( Data?.Thirst > 0f )
		{
			if ( Input.Down( "Run" ) )
				Data.Thirst -= Game.Random.Float( 0.004f, 0.01f );
			else
				Data.Thirst -= 0.001f;
		}

		if ( Data?.Hunger > 0f )
		{
			if ( Input.Down( "Run" ) )
				Data.Hunger -= Game.Random.Float( 0.001f, 0.006f );
			else
				Data.Hunger -= 0.001f;
		}

		TickClothes();

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 200 )
			.UseHitboxes()
			.WithoutTags( "trigger" )
			.WithoutTags( "world" )
			.Ignore( this )
			.Run();

		if ( tr.Entity.IsValid() )
		{
			if ( LastAimEntity != tr.Entity )
			{
				LastAimEntity = tr.Entity;
				Event.Run( OnAimTarget, tr.Entity );
			}
		}
		else
			LastAimEntity = null;

		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );
		CheckButtons();

		if(Game.IsServer)
			CheckAFK( cl );
	}


	public override void FrameSimulate( IClient cl )
	{
		Camera.Rotation = ViewAngles.ToRotation();
		Camera.Position = EyePosition;
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
		Camera.FirstPersonViewer = this;
		UpdateCamera();
	}

	void SimulateAnimation( PawnController controller )
	{
		if ( controller == null )
			return;

		// where should we be rotated to
		var turnSpeed = 0.02f;

		Rotation rotation;

		// If we're a bot, spin us around 180 degrees.
		if ( Client.IsBot )
			rotation = ViewAngles.WithYaw( ViewAngles.yaw + 180f ).ToRotation();
		else
			rotation = ViewAngles.ToRotation();

		var idealRotation = Rotation.LookAt( rotation.Forward.WithZ( 0 ), Vector3.Up );
		Rotation = Rotation.Slerp( Rotation, idealRotation, controller.WishVelocity.Length * Time.Delta * turnSpeed );
		Rotation = Rotation.Clamp( idealRotation, 45.0f, out var shuffle ); // lock facing to within 45 degrees of look direction

		CitizenAnimationHelper animHelper = new CitizenAnimationHelper( this );

		animHelper.WithWishVelocity( controller.WishVelocity );
		animHelper.WithVelocity( controller.Velocity );
		animHelper.WithLookAt( EyePosition + EyeRotation.Forward * 100.0f, 1.0f, 1.0f, 0.5f );
		animHelper.AimAngle = rotation;
		animHelper.FootShuffle = shuffle;
		animHelper.DuckLevel = MathX.Lerp( animHelper.DuckLevel, controller.HasTag( "ducked" ) ? 1 : 0, Time.Delta * 10.0f );
		animHelper.VoiceLevel = (Game.IsClient && Client.IsValid()) ? Client.Voice.LastHeard < 0.5f ? Client.Voice.CurrentLevel : 0.0f : 0.0f;
		animHelper.IsGrounded = GroundEntity != null;
		animHelper.IsSitting = controller.HasTag( "sitting" );
		animHelper.IsNoclipping = controller.HasTag( "noclip" );
		animHelper.IsClimbing = controller.HasTag( "climbing" );
		animHelper.IsSwimming = this.GetWaterLevel() >= 0.5f;
		animHelper.IsWeaponLowered = false;

		if ( controller.HasEvent( "jump" ) ) animHelper.TriggerJump();
		//if ( ActiveChild != lastWeapon ) animHelper.TriggerDeploy();

		if ( ActiveChild is Mbk.RoleplayAPI.Player.BaseCarriable carry )
		{
			carry.SimulateAnimator( animHelper );
		}
		else
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.None;
			animHelper.AimBodyWeight = 0.5f;
		}

		//lastWeapon = ActiveChild;
	}

	/// <summary>
	/// Applies flashbang-like ear ringing effect to the player.
	/// </summary>
	/// <param name="strength">Can be approximately treated as duration in seconds.</param>
	[ClientRpc]
	public void Deafen( float strength )
	{
		Audio.SetEffect( "flashbang", strength, velocity: 20.0f, fadeOut: 4.0f * strength );
	}

	public override void Spawn()
	{
		EnableLagCompensation = true;

		Tags.Add( "player" );

		base.Spawn();
	}

	/// <summary>
	/// Called once the player's health reaches 0.
	/// </summary>
	public override void OnKilled()
	{
		//GameManager.Current?.OnKilled( this );

		ShowDeathHud( To.Single( this ) );
		StopUsing();
		Ragdoll.From( this, Velocity, LastDamage.HasTag( "bullet" ), LastDamage.HasTag( "blast" ), LastDamage.HasTag( "physicsimpact" ), LastDamage.Position, LastDamage.Force, LastDamage.BoneIndex ).FadeOut( 10f );

		LifeState = LifeState.Dead;
		EnableDrawing = false;
		//Delete();
	}


	/// <summary>
	/// Sets LifeState to Alive, Health to Max, nulls velocity, and calls Gamemode.PlayerRespawn
	/// </summary>
	public virtual void Respawn()
	{
		Game.AssertServer();

		Controller = new WalkController();

		if ( DevController is NoclipController )
		{
			DevController = null;
		}

		SetClothes();

		LifeState = LifeState.Alive;
		Health = 100;
		Velocity = Vector3.Zero;

		this.ClearWaterLevel();
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = false;
		EnableTouch = true;

		CreateHull();

		GameManager.Current?.MoveToSpawnpoint( this );
		ResetInterpolation();

		AdminSystem.WriteLog( new LogRespawn( this, Transform, CurrentZone ) );

		GiveInitialItems();

		/*Holster.Add( new PhysGun(), true );
		Holster.Add( new Pistol() );
		Holster.Add( new Fists(), true );
		Holster.Add( new GravGun() );
		Holster.Add( new Flashlight() );
		Holster.Add( new Keys() );
		Holster.Add( new PhoneEntity() );
		Holster.Add( new Wallet() );*/

		if( Game.IsClient )
			_ = new MainHud();
	}

	/// <summary>
	/// Create a physics hull for this player. The hull stops physics objects and players passing through
	/// the player. It's basically a big solid box. It also what hits triggers and stuff.
	/// The player doesn't use this hull for its movement size.
	/// </summary>
	public virtual void CreateHull()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );

		//var capsule = new Capsule( new Vector3( 0, 0, 16 ), new Vector3( 0, 0, 72 - 16 ), 32 );
		//var phys = SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, capsule );


		//	phys.GetBody(0).RemoveShadowController();

		// TODO - investigate this? if we don't set movetype then the lerp is too much. Can we control lerp amount?
		// if so we should expose that instead, that would be awesome.
		EnableHitboxes = true;
	}

	/// <summary>
	/// Called from the gamemode, clientside only.
	/// </summary>
	public override void BuildInput()
	{
		OriginalViewAngles = ViewAngles;
		InputDirection = Input.AnalogMove;

		if ( Input.StopProcessing )
			return;

		var look = Input.AnalogLook;

		if ( ViewAngles.pitch > 90f || ViewAngles.pitch < -90f )
		{
			look = look.WithYaw( look.yaw * -1f );
		}

		var viewAngles = ViewAngles;
		viewAngles += look;
		viewAngles.pitch = viewAngles.pitch.Clamp( -89f, 89f );
		viewAngles.roll = 0f;
		ViewAngles = viewAngles.Normal;

		ActiveChild?.BuildInput();

		GetActiveController()?.BuildInput();

		WorldInput.Ray = AimRay;
		WorldInput.MouseLeftPressed = Input.Down( "Left Click" );
		WorldInput.MouseRightPressed = Input.Down( "Right Click" );
		WorldInput.MouseScroll = Input.MouseWheel;

		if ( WorldInput.Hovered != null )
		{
			Input.Clear( "Left Click" );
			Input.Clear( "Right Click" );
		}
	}

	/// <summary>
	/// A generic corpse entity
	/// </summary>
	public ModelEntity Corpse { get; set; }


	TimeSince timeSinceLastFootstep = 0;

	/// <summary>
	/// A footstep has arrived!
	/// </summary>
	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		if ( LifeState != LifeState.Alive )
			return;

		if ( !Game.IsClient )
			return;

		if ( timeSinceLastFootstep < 0.2f )
			return;

		volume *= FootstepVolume();

		timeSinceLastFootstep = 0;

		//DebugOverlay.Box( 1, pos, -1, 1, Color.Red );
		//DebugOverlay.Text( pos, $"{volume}", Color.White, 5 );

		var tr = Trace.Ray( pos, pos + Vector3.Down * 20 )
			.Radius( 1 )
			.Ignore( this )
			.Run();

		if ( !tr.Hit ) return;

		tr.Surface.DoFootstep( this, tr, foot, volume );
	}

	/// <summary>
	/// Allows override of footstep sound volume.
	/// </summary>
	/// <returns>The new footstep volume, where 1 is full volume.</returns>
	public virtual float FootstepVolume()
	{
		return Velocity.WithZ( 0 ).Length.LerpInverse( 0.0f, 200.0f ) * 0.2f;
	}

	public override void StartTouch( Entity other )
	{
		if ( Game.IsClient ) return;
		if ( TimeSinceDropped < 1 ) return;

		if ( other is PickupTrigger )
		{
			StartTouch( other.Parent );
			return;
		}

		Holster.Add( other, Holster.Active == null );
	}

	/// <summary>
	/// This isn't networked, but it's predicted. If it wasn't then when the prediction system
	/// re-ran the commands LastActiveChild would be the value set in a future tick, so ActiveEnd
	/// and ActiveStart would get called multiple times and out of order, causing all kinds of pain.
	/// </summary>
	[Predicted]
	Entity LastActiveChild { get; set; }

	/// <summary>
	/// Simulated the active child. This is important because it calls ActiveEnd and ActiveStart.
	/// If you don't call these things, viewmodels and stuff won't work, because the entity won't
	/// know it's become the active entity.
	/// </summary>
	public virtual void SimulateActiveChild( IClient cl, Entity child )
	{
		if ( LastActiveChild != child )
		{
			OnActiveChildChanged( LastActiveChild, child );
			LastActiveChild = child;
		}

		if ( !LastActiveChild.IsValid() )
			return;

		if ( LastActiveChild.IsAuthority )
		{
			LastActiveChild.Simulate( cl );
		}
	}

	/// <summary>
	/// Called when the Active child is detected to have changed
	/// </summary>
	public virtual void OnActiveChildChanged( Entity previous, Entity next )
	{
		if ( previous is Mbk.RoleplayAPI.Player.BaseCarriable previousBc )
		{
			previousBc?.ActiveEnd( this, previousBc.Owner != this );
		}

		if ( next is Mbk.RoleplayAPI.Player.BaseCarriable nextBc )
		{
			nextBc?.ActiveStart( this );
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( LifeState == LifeState.Dead )
			return;

		base.TakeDamage( info );

		LastDamage = info;

		this.ProceduralHitReaction( info );

		//
		// Add a score to the killer
		//
		if ( LifeState == LifeState.Dead && info.Attacker != null )
		{
			if ( info.Attacker.Client != null && info.Attacker != this )
			{
				info.Attacker.Client.AddInt( "kills" );
			}
		}

		if ( info.HasTag( "blast" ) )
		{
			Deafen( To.Single( Client ), info.Damage.LerpInverse( 0, 60 ) );
		}
	}

	public override void OnChildAdded( Entity child )
	{
		Holster?.OnChildAdded( child );
	}

	public override void OnChildRemoved( Entity child )
	{
		Holster?.OnChildRemoved( child );
	}

	/// <summary>
	/// Position a player should be looking from in world space.
	/// </summary>
	public Vector3 EyePosition
	{
		get => Transform.PointToWorld( EyeLocalPosition );
		set => EyeLocalPosition = Transform.PointToLocal( value );
	}

	/// <summary>
	/// Position a player should be looking from in local to the entity coordinates.
	/// </summary>
	[Net, Predicted]
	public Vector3 EyeLocalPosition { get; set; }

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity.
	/// </summary>
	public Rotation EyeRotation
	{
		get => Transform.RotationToWorld( EyeLocalRotation );
		set => EyeLocalRotation = Transform.RotationToLocal( value );
	}

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity. In local to the entity coordinates.
	/// </summary>
	[Net, Predicted]
	public Rotation EyeLocalRotation { get; set; }

	/// <summary>
	/// Override the aim ray to use the player's eye position and rotation.
	/// </summary>
	public override Ray AimRay => new Ray( EyePosition, EyeRotation.Forward );

	public void SetNoclip()
	{
		if ( DevController is NoclipController )
		{
			DevController = null;
		}
		else
		{
			DevController = new NoclipController();
		}
	}

	[ConCmd.Server( "holster_current" )]
	public static void SetHolsterCurrent( string entName )
	{
		var target = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		if ( target == null ) return;

		var holder = target.Holster;
		if ( holder == null )
			return;

		for ( int i = 0; i < holder.Count(); ++i )
		{
			var slot = holder.GetSlot( i );
			if ( !slot.IsValid() )
				continue;

			if ( slot.ClassName != entName )
				continue;

			holder.SetActiveSlot( i, false );

			break;
		}
	}

	private void CheckAFK(IClient client)
	{
		if ( IsPressingButton )
		{
			TimeSinceAFK = 0f;
		}

		if ( IsPressingButton && IsAFK )
		{
			TimeSinceAFK = 0f;
			IsAFK = false;
			Chat.AddChat(To.Single( client ), "AFK System", "You are not AFK anymore !" );
			HideAFKUI( To.Single( client ) );
			Event.Run( OnUnAFK );
			return;
		}

		if ( !IsPressingButton && !IsAFK && TimeSinceAFK >= RoleplayAPI.Instance.Configuration.AfkDelay )
		{
			IsAFK = true;
			Chat.AddChat( To.Single( client ), "AFK System", "You are now AFK !" );
			ShowAFKUI( To.Single( client ) );
			Event.Run( OnAFK );
			return;
		}

		if( TimeSinceAFK >= RoleplayAPI.Instance.Configuration.AfkDelayKick )
		{
			client.Kick();
			AdminSystem.WriteLog( new LogKick( User.Get( client ), User.Get( client ), "Kicked for inactivity" ) );
		}
	}
}
