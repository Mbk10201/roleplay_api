using Mbk.Admin;
using Mbk.Admin.Logs;
using Mbk.Admin.UI.Dialog.Childs;
using Mbk.Discord;
using Mbk.Discord.Attributes;
using Mbk.Discord.Models;
using Mbk.RoleplayAPI.Jobs.List;
using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.UI.Shared.AlertSystem;
using Mbk.RoleplayAPI.UI.Shared.Chat;
using Mbk.RoleplayAPI.World;

namespace Mbk.RoleplayAPI.Jobs;

[Library]
[Display( Name = "Job system" ), Category( "Roleplay" ), Icon( "work" )]
public partial class JobSystem : Entity
{
	public static JobSystem Instance { get; private set; }

	[Net] public IList<Job> Jobs { get; private set; }

	public JobSystem()
	{
		Instance = this;

		if ( Game.IsServer )
			Configure();
	}

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		base.Spawn();
	}

	public void Configure()
	{
		Jobs = new List<Job>()
		{
			new JobDefault(),
			new JobPolice(),
			new JobHospital()
		};

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

	[DiscordGameEvent( "Client demote", "client_demote", "When a admin changes a players health." )]
	[Command( "Demote", typeof( NotImplementedDialog ), "ic:sharp-work-off", clientaction: true )]
	public static void Demote( long steamid, long adminid )
	{
		var client = Game.Clients.SingleOrDefault( x => x.SteamId == steamid );
		var admin = Game.Clients.SingleOrDefault( x => x.SteamId == adminid );

		if ( client == null )
			return;

		if ( client.Pawn == null )
			return;

		if ( !admin.CanTarget( client ) )
		{
			Mbk.Admin.UI.Alert.Alert.Add( To.Single( admin ), "Impossible", $"{client.Name} has more immunity than you !", Mbk.Admin.UI.Alert.eAlertType.Error );
			return;
		}

		var pawn = client.Pawn as RoleplayPlayer;
		pawn.Data.JobId = "default";
		pawn.Data.GradeId = 1;
		Mbk.Admin.UI.Alert.Alert.Add( To.Single( admin ), "Success", $"{admin.Name} has demoted {client.Name}.", Mbk.Admin.UI.Alert.eAlertType.Success );
		AdminSystem.WriteLog( new LogDemote(Mbk.Admin.User.Get(client), Mbk.Admin.User.Get(admin)) );

		var EventSettings = DiscordSystem.GetGameEvent( "client_demote" );

		if ( EventSettings.Broadcast )
		{
			var message = new MessageForm();

			if ( EventSettings.DisplayEmbed )
			{
				message = new()
				{
					Embeds = new()
					{
						new Embed()
						{
							Title = EventSettings.Name,
							Description = $"{admin.Name} has demoted {client.Name}.",
							Color = EventSettings.GetColor()
						}
					}
				};
			}
			else
			{
				message = new()
				{
					Content = $"{admin.Name} has demoted {client.Name}."
				};
			}

			if ( EventSettings.UseAsBot && Discord.Client.Instance.TokenValid )
			{
				if ( EventSettings.ChannelID is null )
					return;

				Discord.Client.SendMessage( EventSettings.ChannelID.Value, message );
			}
			else
			{
				if ( EventSettings.Webhook == string.Empty )
					return;

				Webhook.SendMessage( EventSettings.Webhook, message );
			}
		}
	}

	[DiscordGameEvent( "Set job", "set_job", "When a admin set job of the player." )]
	[Command( "Set Job", typeof( NotImplementedDialog ), "ic:baseline-work", clientaction: true )]
	public static void SetJob( long steamid, long adminid, string job, int grade )
	{
		var client = Game.Clients.SingleOrDefault( x => x.SteamId == steamid );
		var admin = Game.Clients.SingleOrDefault( x => x.SteamId == adminid );

		if ( client == null )
			return;

		var pawn = client.Pawn as RoleplayPlayer;

		if ( pawn == null )
			return;

		if ( !admin.CanTarget( client ) )
		{
			Mbk.Admin.UI.Alert.Alert.Add( To.Single( admin ), "Impossible", $"{client.Name} has more immunity than you !", Mbk.Admin.UI.Alert.eAlertType.Error );
			return;
		}

		AdminSystem.WriteLog( new LogJobChange( Mbk.Admin.User.Get( client ), Mbk.Admin.User.Get( admin ), pawn.Data.JobId, job, pawn.Data.GradeId, grade ) );

		pawn.Data.JobId = job;
		pawn.Data.GradeId = grade;

		Alert.Add( To.Single( pawn ), new()
		{
			Message = $"You are now working as: {JobSystem.GetJobName( job )}"
		} );
	}

	public static void TeleportToJob( RoleplayPlayer player )
	{
		var job = player.Data.JobId;
		var query = Instance.Jobs.Single(j => j.Identifier == job);
		var vec = query.Spawns[Game.Random.Int( 0, query.Spawns.Count )];

		if( query.Spawns.Count != 0 && vec != Vector3.Zero)
			player.Position = vec;
	}

	[WeatherSystem.OnNewDay]
	public void OnNewDay(ushort day)
	{
		GiveSalary();
	}

	public static void GiveSalary()
	{
		Event.Run( OnGiveSalary );
		
		foreach( var player in Entity.All.OfType<RoleplayPlayer>() )
		{
			int salary = GetGradeSalary( player.Data.JobId, player.Data.GradeId );

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
