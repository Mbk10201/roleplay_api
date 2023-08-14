using Mbk.RoleplayAPI.Jobs.List;
using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.UI.Shared.AlertSystem;
using Mbk.RoleplayAPI.UI.Shared.Chat;

namespace Mbk.RoleplayAPI.Jobs;

[Library]
[Display( Name = "Job system" ), Category( "Roleplay" ), Icon( "work" )]
public partial class JobSystem : Entity
{
	public static JobSystem Instance { get; private set; }

	[Net] public IList<Job> Jobs { get; private set; }

	[GameEvent.Entity.PostSpawn]
	public static void OnPostSpawn()
	{
		Game.AssertServer();
		_ = new JobSystem();
	}

	public JobSystem()
	{
		if ( RoleplayAPI.Debug() )
			Log.Info( "JobSystem - Initialize" );

		Instance = this;
		Transmit = TransmitType.Always;

		if ( Game.IsServer )
			Configure();
	}
	
	public void Configure()
	{
		Jobs = new List<Job>();

		Jobs.Add( new JobDefault() );
		Jobs.Add( new JobPolice() );
		/*Jobs.Add( new JobHospital() );
		Jobs.Add( new JobFireFighter() );*/

		/*foreach ( var type in TypeLibrary.GetTypes().Where( x => x.IsClass && !x.IsAbstract) )
		{
			if(type.Name == "JobDefault")
			{
				Log.Info( type.TargetType );
			}

			if( type.TargetType.BaseType is not null)
			{
				Log.Info(type.TargetType.BaseType.Name);
			}
		}*/
	}

	[ConCmd.Server]
	public static void SetJob( string job, int grade )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;

		if ( player is null )
			return;

		//LogSystem.Write( new LogJobChange( player, player, player.Data.JobID, job, player.Data.GradeID, grade ) );
		player.Data.JobID = job;
		player.Data.GradeID = grade;
	}

	[ConCmd.Server]
	public static void SetJob( int playerid, string job, int grade )
	{
		var player = Entity.All.Single(x => x.NetworkIdent == playerid) as RoleplayPlayer;

		if ( player is null )
			return;
		
		//LogSystem.Write( new LogJobChange( player, player, player.Data.JobID, job, player.Data.GradeID, grade ) );

		player.Data.JobID = job;
		player.Data.GradeID = grade;

		Alert.Add( To.Single( player ), new()
		{
			Message = $"You are now working as: {JobSystem.GetJobName( job )}"
		} );
	}

	public static void TeleportToJob( RoleplayPlayer player )
	{
		var job = player.Data.JobID;
		var query = Instance.Jobs.Single(j => j.Identifier == job);
		var vec = query.Spawns[Game.Random.Int( 0, query.Spawns.Count )];

		if( query.Spawns.Count != 0 && vec != Vector3.Zero)
			player.Position = vec;
	}

	public static void GiveSalary()
	{
		Event.Run( OnGiveSalary );
		
		foreach( var player in Entity.All.OfType<RoleplayPlayer>() )
		{
			int salary = GetGradeSalary( player.Data.JobID, player.Data.GradeID );

			if ( player.Data.HasBankDetails)
				player.Data.Bank += salary;
			else
				player.Data.Money += salary;

			Alert.Add( To.Single( player ), new ()
			{
				Message = $"Vous avez reçu votre salaire de {salary}€"
			} );
		}
	}
}
