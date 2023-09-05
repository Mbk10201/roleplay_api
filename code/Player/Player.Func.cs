﻿using Mbk.Admin;
using Mbk.Admin.UI.Alert;
using Mbk.Discord;
using Mbk.Discord.Attributes;
using Mbk.Discord.Models;

namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	[DiscordGameEvent( "Set money", "set_money", "When a admin set money of a player." )]
	[Command( "Set Money", typeof( SetMoneyDialog ), "material-symbols:euro", clientaction: true )]
	public static void SetMoney( long steamid, long adminid, long value )
	{
		var client = Game.Clients.SingleOrDefault( x => x.SteamId == steamid );
		var admin = Game.Clients.SingleOrDefault( x => x.SteamId == adminid );

		if ( !admin.CanTarget( client ) )
		{
			Alert.Add( To.Single( admin ), "Impossible", $"{client.Name} has more immunity than you !", Mbk.Admin.UI.Alert.eAlertType.Error );
			return;
		}

		Alert.Add( To.Single( admin ), "Success", $"You have successfully set money of {client.Name} to {value}", Mbk.Admin.UI.Alert.eAlertType.Success );

		var pawn = client.Pawn as RoleplayPlayer;
		pawn.Data.Money = value;

		var EventSettings = DiscordSystem.GetGameEvent( "set_money" );

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
							Description = $"{admin.Name} has set the money of {client.Name} to {value}€.",
							Color = EventSettings.GetColor()
						}
					}
				};
			}
			else
			{
				message = new()
				{
					Content = $"{admin.Name} has set the money of {client.Name} to {value}€."
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

	[ConCmd.Server]
	public static void AddMoney( long value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Money += value;
	}

	[ConCmd.Server]
	public static void SetBank( long value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Bank = value;
	}

	[ConCmd.Server]
	public static void AddBank( long value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Bank += value;
	}

	[ConCmd.Server]
	public static void SetFirstname( string value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.FirstName = value;
	}

	[ConCmd.Server]
	public static void SetLastname( string value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.LastName = value;
	}

	[ConCmd.Server]
	public static void SetLevel( int value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Level = value;
	}

	[ConCmd.Server]
	public static void SetXP( int value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.XP = value;
	}

	[ConCmd.Server]
	public static void AddXP( int value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.XP += value;
	}

	[ConCmd.Server]
	public static void SetJobID( string value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.JobId = value;
	}

	[ConCmd.Server]
	public static void SetJobGradeID( int value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.GradeId = value;
	}

	[ConCmd.Server]
	public static void SetHunger( float value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Hunger = value;
	}

	[ConCmd.Server]
	public static void SetThirst( float value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Thirst = value;
	}

	[ClientRpc]
	public void AddMapMarker( Vector3 position, Color color )
	{
		Markers.Add( new MapMarker { Position = position, Color = color } );
	}

	[ConCmd.Server]
	public static void AddMapMarker( string csv, string hex )
	{
		if ( ConsoleSystem.Caller.Pawn is not RoleplayPlayer player ) return;

		var position = csv.ToVector3();
		var color = Color.Parse( hex ).Value.WithAlpha( 1f );

		var marker = new MapMarker();
		marker.Position = position;
		marker.Color = color;

		player.Markers.Add( marker );

		player.AddMapMarker( To.Single( player ), position, color );
	}

	[ConCmd.Server]
	public static void RemoveMapMarker( string csv )
	{
		if ( ConsoleSystem.Caller.Pawn is not RoleplayPlayer player ) return;

		var position = csv.ToVector3();

		foreach ( var marker in player.Markers )
		{
			if ( marker.Position.Distance( position ) <= 100f )
			{
				player.Markers.Remove( marker );
				break;
			}
		}

		player.RemoveMapMarker( To.Single( player ), position );
	}

	[ClientRpc]
	public void RemoveMapMarker( Vector3 position )
	{
		foreach ( var marker in Markers )
		{
			if ( marker.Position.Distance( position ) <= 100f )
			{
				Markers.Remove( marker ); 
				break;
			}
		}
	}

	private void  CheckButtons()
	{
		string pressed = "";
		
		if ( Input.Down( "Left Click") )
		{
			pressed = "Left Click";
		}

		if ( Input.Down( "Right Click" ) )
		{
			pressed = "Right Click";
		}

		if ( Input.Down( "Reload" ) )
		{
			pressed = "Reload";
		}

		if ( Input.Down( "Use" ) )
		{
			pressed = "Use";
		}

		if ( Input.Down( "Drop" ) )
		{
			pressed = "Drop";
		}

		if ( Input.Down( "Slot0" ) )
		{
			pressed = "Slot0";
		}

		if ( Input.Down( "Slot1" ) )
		{
			pressed = "Slot1";
		}

		if ( Input.Down( "Slot2" ) )
		{
			pressed = "Slot2";
		}

		if ( Input.Down( "Slot3" ) )
		{
			pressed = "Slot3";
		}

		if ( Input.Down( "Slot4" ) )
		{
			pressed = "Slot4";
		}

		if ( Input.Down( "Slot5" ) )
		{
			pressed = "Slot5";
		}

		if ( Input.Down( "Slot6" ) )
		{
			pressed = "Slot6";
		}

		if ( Input.Down( "Slot7" ) )
		{
			pressed = "Slot7";
		}

		if ( Input.Down( "Slot8" ) )
		{
			pressed = "Slot8";
		}

		if ( Input.Down( "Slot9" ) )
		{
			pressed = "Slot9";
		}

		if ( Input.Down( "SlotPrev" ) )
		{
			pressed = "SlotPrev";
		}

		if ( Input.Down( "SlotNext" ) )
		{
			pressed = "SlotNext";
		}

		if ( Input.Down( "Forward" ) )
		{
			pressed = "Forward";
		}

		if ( Input.Down( "Backward" ) )
		{
			pressed = "Backward";
		}

		if ( Input.Down( "Left" ) )
		{
			pressed = "Left";
		}

		if ( Input.Down( "Right" ) )
		{
			pressed = "Right";
		}

		if ( Input.Down( "Jump" ) )
		{
			pressed = "Jump";
		}

		if ( Input.Down( "Run" ) )
		{
			pressed = "Run";
		}

		if ( Input.Down( "Walk" ) )
		{
			pressed = "Walk";
		}

		if ( Input.Down( "Duck" ) )
		{
			pressed = "Duck";
		}

		if ( Input.Down( "View" ) )
		{
			pressed = "View";
		}

		if ( Input.Down( "Voice" ) )
		{
			pressed = "Voice";
		}

		if ( Input.Down( "Tab" ) )
		{
			pressed = "Tab";
		}

		if ( Input.Down( "Menu" ) )
		{
			pressed = "Menu";
		}

		if ( Input.Down( "Chat" ) )
		{
			pressed = "Chat";
		}

		if ( Input.Down( "Inventory" ) )
		{
			pressed = "Inventory";
		}

		IsPressingButton = pressed != "";
		IsPressingButtonName = pressed;
	}
}
