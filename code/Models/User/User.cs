using Mbk.RoleplayAPI.Database;
using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.Models;

public partial class User : BaseNetworkable
{
	private RoleplayPlayer Player { get; set; }

	[Net, JsonPropertyName( "id")]
    public int Id { get; set; }

	[Net, JsonPropertyName( "steamid" )]
	public long SteamID { get; set; }

	[Net, JsonPropertyName( "name" )]
	public string Name { get; set; }

	[Net, JsonPropertyName( "firstname" )]
	public string Firstname { get; set; }

	[Net, JsonPropertyName( "lastname" )]
	public string Lastname { get; set; }

	[Net, JsonPropertyName( "nationality" )]
	public int Nationality { get; set; }

	[Net, JsonPropertyName( "money" )]
	public int Money { get; set; }

	[Net, JsonPropertyName( "bank" )] 
	public int Bank { get; set; }

	[Net, JsonPropertyName( "level" )] 
	public int Level { get; set; }

	[Net, JsonPropertyName( "xp" )] 
	public int XP { get; set; }

	[Net, JsonPropertyName( "job_id" )]
	public string JobID { get; set; }

	[Net, JsonPropertyName( "grade_id" )]
	public int GradeID { get; set; }

	[Net, JsonPropertyName( "hunger" )]
	public float Hunger { get; set; } = 100f;

	[Net, JsonPropertyName( "thirst" )]
	public float Thirst { get; set; } = 100f;

	[Net, JsonPropertyName( "has_bank_card" )]
	public bool HasBankCard { get; set; }

	[Net, JsonPropertyName( "has_bank_details" )] 
	public bool HasBankDetails { get; set; }

	[Net, JsonPropertyName( "is_new")]
	public bool IsNew { get; set; }

	[Net, JsonPropertyName( "stats" )]
	public UserStats Stats { get; set; } = new();

	[Net, JsonPropertyName( "preferences" )]
	public UserPreferences Preferences { get; set; } = new();

	[Net, JsonPropertyName( "character" )]
	public UserCharacter Character { get; set; } = new();

	[JsonIgnore]
	private const string WebRoute = "game/users";

	public User()
	{

	}

	public User( RoleplayPlayer _player ) : this()
	{
		Player = _player;
	}

	public async Task Init()
	{
		Game.AssertServer();

		if ( await HasAccount() )
		{
			if ( !await Load() )
				Log.Info( $"[Database] Account load for {Player.Client.SteamId} has failed" );
			else
				Log.Info( $"[Database] Account load for {Player.Client.SteamId} has succeeded" );
		}
		else
		{
			if ( !await Create() )
				Log.Info( $"[Database] Account creation for {Player.Client.SteamId} has failed" );
			else
				Log.Info( $"[Database] Account creation for {Player.Client.SteamId} has succeeded" );
		}
	}

	public async Task<bool> Load()
	{
		Game.AssertServer();
		var result = await Request.Get<User>( $"{WebRoute}/get?steamid={Player.Client.SteamId}" );

		Log.Info( result );

		Player.Data = result;

		if ( result is not null )
			return true;
		else
			return false;
	}

	public async Task TryToSave()
	{
		Game.AssertServer();

		if(!await Save())
			Log.Info( $"[Database] Account save for {Player.Client.SteamId} has failed" );
		else
			Log.Info( $"[Database] Account save for {Player.Client.SteamId} has succeeded" );
	}

	public async Task<bool> Save()
	{
		Game.AssertServer();

		var result = await Request.Post<User>( $"{WebRoute}/update", this );
		Log.Info( result );

		if ( result.IsSuccessStatusCode )
			return true;
		else
			return false;
	}

	public async Task<bool> Create()
	{
		Game.AssertServer();

		Player.Data.IsNew = true;

		var result = await Request.Post<User>( $"{WebRoute}/add", new User()
		{
			SteamID = Player.Client.SteamId,
			Name = Player.Client.Name,
			Stats = new(),
			Preferences = new(),
			Character = new()
		} );

		if ( result.IsSuccessStatusCode )
			return true;
		else
			return false;
	}

	public async Task<bool> CheckIfExist()
	{
		Game.AssertServer();

		return await Request.Get<bool>( $"{WebRoute}/exist?steamid={Player.Client.SteamId}" );
	}

	/// <summary>
	/// Perform a database query to check if the client entry exist in the database
	/// Return true if exist or false if not
	/// </summary>
	private async Task<bool> HasAccount()
	{
		Game.AssertServer();

		if ( await CheckIfExist() )
		{
			Log.Info( $"User {Player.Client.SteamId} exists in database" );
			return true;
		}
		else
		{
			Log.Info( $"User {Player.Client.SteamId} doesn't exist in database" );
			return false;
		}
	}
}
