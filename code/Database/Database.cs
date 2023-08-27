using Mbk.RoleplayAPI.Database.DTO;

using System.Collections.ObjectModel;


namespace Mbk.RoleplayAPI.Database;

public static class Database
{
	public static IList<ITable> Tables { get; set; }

	public static void Initialize()
	{
		Tables = new List<ITable>();

		Log.Info( "Initialize" );

		foreach (var type in TypeLibrary.GetTypes().Where( x => x.IsClass))
		{
			if( type.Interfaces.Where(x => x.Name == "ITable").Any() )
			{
				//var table = TypeLibrary.Create<ITable>( type.TargetType.Name );
				var table = type.Create<ITable>( );
				table.Load();
				Tables.Add( table );
			}
		}
	}
	
	public static T Get<T>() where T : ITable
	{
		return Tables.OfType<T>().FirstOrDefault();
	}
}

public interface ITable
{
	string Name { get; }
	string Controller { get; }

	Task Load();
	Task Save();
	Task Insert<T>( T value );
	Task<bool> Update<T>( T value );
}

public class PlayerTable : ITable
{
	public string Name => "Player";
	public string Controller => "game/users";

	public ICollection<UserDTO> Rows { get; set; }

	public PlayerTable()
	{
		Rows = new Collection<UserDTO>();
	}

	public async Task Load()
	{
		Log.Info( "[Database] PlayerTable: Load()" );

		Rows?.Clear();
		Rows = await Request.Get<ICollection<UserDTO>>( $"{Controller}/getall" );

		foreach(var x in Rows)
		{
			Log.Info( x.Name );
			Log.Info( x.SteamId );
		}
	}

	public async Task Save()
	{
		Log.Info( "[Database] PlayerTable: SaveAll()" );

		using var response = await Request.Post( $"{Controller}/saveall", Rows );
		response.EnsureSuccessStatusCode();
	}

	public async Task Insert<T>(T Value)
	{
		var user = Value as UserDTO;
		
		if( user == null)
			return;

		Log.Info( user );

		Rows.Add( user );
		await Request.Post( $"{Controller}/add", user );
	}

	public async Task<bool> Update<T>( T Value )
	{
		Log.Info( Value );

		throw new NotImplementedException();
	}

	public UserDTO Get( IClient client )
	{
		if ( Rows.Count == 0 )
			return null;

		return Rows.SingleOrDefault( x => x.SteamId == client.SteamId, new UserDTO() );
	}

	public UserDTO Get( long steamid )
	{
		if ( Rows.Count == 0 )
			return null;

		return Rows.SingleOrDefault( x => x.SteamId == steamid, new UserDTO() );
	}

	public UserDTO Get( Guid id )
	{
		if ( Rows.Count == 0 )
			return null;

		return Rows.SingleOrDefault( x => x.Id == id, new UserDTO() );
	}

	public bool Exist( long steamid )
	{
		if ( Rows.Count == 0 )
			return false;

		Log.Info( $"Exists: {steamid}" );

		return Rows.SingleOrDefault( x => x.SteamId == steamid ) != null;
	}
}
