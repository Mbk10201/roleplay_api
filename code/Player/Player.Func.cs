namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	[ConCmd.Server]
	public static void SetMoney( long value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Money = value;
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
		player.Data.Firstname = value;
	}

	[ConCmd.Server]
	public static void SetLastname( string value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.Lastname = value;
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
		player.Data.JobID = value;
	}

	[ConCmd.Server]
	public static void SetJobGradeID( int value )
	{
		var player = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		player.Data.GradeID = value;
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
}
