using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.UI.WorldPanels;

/// <summary>
/// When a player is within radius of the camera we add this to their entity.
/// We remove it again when they go out of range.
/// </summary>
internal class NameTagComponent : EntityComponent<RoleplayPlayer>
{
	NameTagPanel NameTagPanel;

	protected override void OnActivate()
	{
		NameTagPanel = new NameTagPanel( Entity.Client );
	}

	protected override void OnDeactivate()
	{
		NameTagPanel?.Delete();
		NameTagPanel = null;
	}

	/// <summary>
	/// Called for every tag, while it's active
	/// </summary>
	[GameEvent.Client.Frame]
	public void FrameUpdate()
	{
		var tx = Entity.GetAttachment( "hat" ) ?? Entity.Transform;
		tx.Position += Vector3.Up * 10.0f;
		tx.Rotation = Rotation.LookAt( -Screen.GetDirection( new Vector2( Screen.Width * 0.5f, Screen.Height * 0.5f ) ) );

		NameTagPanel.Transform = tx;
	}

	/// <summary>
	/// Called once per frame to manage component creation/deletion
	/// </summary>
	[GameEvent.Client.Frame]
	public static void SystemUpdate()
	{
		foreach ( var player in Sandbox.Entity.All.OfType<RoleplayPlayer>() )
		{
			if ( player.Position.Distance( Camera.Position ) > 300 || player.LifeState == LifeState.Dead )
			{
				var c = player.Components.Get<NameTagComponent>();
				c?.Remove();
				continue;
			}

			if ( player != Game.LocalPawn as RoleplayPlayer )
				player.Components.GetOrCreate<NameTagComponent>();
		}
	}
}
