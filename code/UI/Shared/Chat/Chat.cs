using Mbk.Discord.Attributes;
using Mbk.Discord.Models;
using Mbk.Discord;
using Mbk.RoleplayAPI.UI.RootPanels;
using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.UI.Shared.AlertSystem;
using Mbk.RoleplayAPI.Entities;
using Mbk.Admin;
using Mbk.Admin.Logs;

namespace Mbk.RoleplayAPI.UI.Shared.Chat;

public partial class Chat
{

	[ConCmd.Client]
	public static void Say( string message )
	{
		if ( message[0] == RoleplayAPI.Instance.Configuration.ChatCommandPrefix )
		{
			Chat.ExecuteCommand(Game.LocalClient, message );
		}

		Say2( message );
	}

	[ConCmd.Server]
	public static void Say2( string message )
	{
		Event.Run( RoleplayAPI.OnNewChatMessage, ConsoleSystem.Caller.Pawn, message );

		if ( !ConsoleSystem.Caller.IsValid() ) return;

		if ( message[0] == RoleplayAPI.Instance.Configuration.ChatCommandPrefix )
		{
			Chat.ExecuteCommand( ConsoleSystem.Caller, message );
			return;
		}

		SendToDiscord( ConsoleSystem.Caller.Name, message );

		AdminSystem.WriteLog( new LogChat( ConsoleSystem.Caller.Pawn as Entity, message ) );

		AddChat( To.Everyone, ConsoleSystem.Caller.Name, message );
	}

	[ConCmd.Client( "rp_chat_add", CanBeCalledFromServer = true )]
	public static void AddChat( string name, string message, string classes = "" )
	{
		MainHud.ChatPanel.AddMessage( name, message, classes );
	}

	[GameEvent.Client.BuildInput]
	public static void BuildInput()
	{
		if ( Input.Pressed( "Chat" ) )
		{
			if ( !MainHud.ChatPanel.IsOpen )
			{
				MainHud.ChatPanel.Open();
			}
		}
	}

	[DiscordGameEvent( "Chat message", "chat_message", "When a client write in the chat" )]
	private static void SendToDiscord( string username, string message )
	{
		var EventSettings = DiscordSystem.GetGameEvent( "chat_message" );

		if ( !EventSettings.Broadcast )
			return;

		var messageObj = new MessageForm();

		if ( EventSettings.DisplayEmbed )
		{
			messageObj = new()
			{
				Embeds = new()
				{
					new Embed()
					{
						Title = EventSettings.Name,
						Description = $"{username}: {message}",
						Color = EventSettings.GetColor()
					}
				}
			};
		}
		else
		{
			messageObj = new()
			{
				Content = $"{username}: {message}"
			};
		}

		if ( EventSettings.UseAsBot && Client.IsClientLogged() )
		{
			if ( EventSettings.ChannelID is null )
				return;

			Client.SendMessage( EventSettings.ChannelID.Value, messageObj );
		}
		else
		{
			if ( EventSettings.Webhook == string.Empty )
				return;

			Webhook.SendMessage( EventSettings.Webhook, messageObj );
		}
	}

	public static async void ExecuteCommand( IClient cl, string command )
	{
		Assert.True( cl.IsValid() );

		if ( string.IsNullOrWhiteSpace( command ) ) return;
		if ( command[0] != RoleplayAPI.Instance.Configuration.ChatCommandPrefix ) return;

		var args = command.Remove( 0, 1 ).Split( ' ' );
		var cmdName = args[0].ToLower();

		var player = cl.Pawn as RoleplayPlayer;

		if ( Game.IsServer )
		{
			switch ( cmdName )
			{
				case "givesalary":
				{
					//JobCore.GiveSalary();
					break;
				}
				case "kill":
				{
					player.TakeDamage( new DamageInfo { Damage = player.Health * 99 } );
					break;
				}
				case "noclip":
				{
					Log.Info( "noclip" );
					player.SetNoclip();

					break;
				}
				case "atm":
				{
					var tr = Trace.Ray( player.AimRay, 256f )
					.Size( 1.0f )
					.Ignore( player )
					.Run();

					if ( tr.Hit )
						ConsoleSystem.Run( $"CreateAtm {tr.EndPosition}" );
					break;
				}
			}
		}

		if ( Game.IsClient )
		{
			switch ( cmdName )
			{
				case "job":
				{
					//TempMenu.Current.SetClass( "open", true );
					break;
				}
				case "noclip":
				{
					Log.Info( "noclip" );
					player.SetNoclip();
					break;
				}
				case "info":
				{
					Alert.Add( new()
					{
						Title = "Info",
						Message = "Hello this is a info alert test",
						Type = Models.Enums.eAlertType.Info
					} );
					break;
				}
				case "success":
				{
					Alert.Add( new()
					{
						Title = "Success",
						Message = "Hello this is a success alert test",
						Type = Models.Enums.eAlertType.Success
					} );
					break;
				}
				case "error":
				{
					Alert.Add( new()
					{
						Title = "Error",
						Message = "Hello this is a error alert test",
						Type = Models.Enums.eAlertType.Error
					} );
					break;
				}
				case "warning":
				{
					Alert.Add( new()
					{
						Title = "Warning",
						Message = "Hello this is a warning alert test",
						Type = Models.Enums.eAlertType.Warning
					} );
					break;
				}
				case "custom":
				{
					Alert.Add( new()
					{
						Title = "Custom",
						Message = "Hello this is a custom alert test",
						Type = Models.Enums.eAlertType.Custom
					} );
					break;
				}
				case "voiture":
				case "car":
				case "vehicle":
				{
					CarEntity.SpawnCar( player.Position + Vector3.Right * 150f );
					break;
				}
				case "admin":
				{
					AdminSystem.Toggle();
					break;
				}
				case "showvehiclehud":
				{
					Vehicles.Vehicles.ShowHud();
					break;
				}
			}
		}

		Event.Run( RoleplayAPI.OnExecuteCommand, cmdName );
	}
}
