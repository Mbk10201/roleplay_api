using Mbk.RoleplayAPI.Player;
using Sandbox;

namespace Mbk.RoleplayAPI;

public partial class RoleplayAPI
{
	[ConCmd.Server]
	public static void Respawn()
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;

		player.Respawn();
	}

	[ConCmd.Server]
	public static void Respawn( int playerid )
	{
		var player = Entity.All.Single( x => x.NetworkIdent == playerid ) as RoleplayPlayer;

		if ( player is null )
			return;

		player.Respawn();
	}
}
