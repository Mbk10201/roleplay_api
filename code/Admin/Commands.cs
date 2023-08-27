using Mbk.Admin;
using Mbk.Discord.Attributes;
using Mbk.Admin.UI.Dialog.Childs;
using Mbk.Admin.UI.Alert;
using Mbk.Discord;
using Mbk.Discord.Models;
using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.Admin;

internal static class Commands
{
	[DiscordGameEvent( "Noclip", "noclip", "When a admin unfreeze a player." )]
	[Command( "Noclip", typeof(NotImplementedDialog), "game-icons:flying-fox", clientaction: true )]
	public static void Noclip( long steamid, long adminid )
	{
		var client = Game.Clients.SingleOrDefault( x => x.SteamId == steamid );
		var admin = Game.Clients.SingleOrDefault( x => x.SteamId == adminid );

		if ( client == null )
			return;

		if ( client.Pawn == null )
			return;

		if ( !admin.CanTarget( client ) )
		{
			Alert.Add( To.Single( admin ), "Impossible", $"{client.Name} has more immunity than you !", Mbk.Admin.UI.Alert.eAlertType.Error );
			return;
		}

		var pawn = client.Pawn as RoleplayPlayer;
		var messageformat = "";

		if ( pawn.DevController is Player.NoclipController)
		{
			pawn.SetNoclip();
			messageformat = $"You have successfully unset noclip to {client.Name}";
			Alert.Add( To.Single( admin ), "Success", messageformat, Mbk.Admin.UI.Alert.eAlertType.Success );
		}
		else
		{
			pawn.SetNoclip();
			messageformat = $"You have successfully set noclip to {client.Name}";
			Alert.Add( To.Single( admin ), "Success", messageformat, Mbk.Admin.UI.Alert.eAlertType.Success );
		}

		var EventSettings = DiscordSystem.GetGameEvent( "noclip" );

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
							Description = messageformat,
							Color = EventSettings.GetColor()
						}
					}
				};
			}
			else
			{
				message = new()
				{
					Content = messageformat,
				};
			}

			if ( EventSettings.UseAsBot && Client.Instance.TokenValid )
			{
				if ( EventSettings.ChannelID is null )
					return;

				Client.SendMessage( EventSettings.ChannelID.Value, message );
			}
			else
			{
				if ( EventSettings.Webhook == string.Empty )
					return;

				Webhook.SendMessage( EventSettings.Webhook, message );
			}
		}
	}
}
