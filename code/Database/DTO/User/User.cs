using Mbk.RoleplayAPI.Database;
using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.Database.DTO;

public partial class UserDTO : BaseNetworkable
{
	[Net]
    public Guid Id { get; set; }

	[Net]
	public long SteamId { get; set; }

	[Net]
	public string Name { get; set; }

	[Net]
	public string FirstName { get; set; }

	[Net]
	public string LastName { get; set; }

	[Net]
	public int Nationality { get; set; }

	[Net]
	public long Money { get; set; } = 50000;

	[Net] 
	public long Bank { get; set; }

	[Net] 
	public int Level { get; set; }

	[Net] 
	public int XP { get; set; }

	[Net]
	public string JobId { get; set; } = "default";

	[Net]
	public int GradeId { get; set; } = 1;

	[Net]
	public float Hunger { get; set; } = 100f;

	[Net]
	public float Thirst { get; set; } = 100f;

	[Net]
	public bool HasBankCard { get; set; }

	[Net] 
	public bool HasBankDetails { get; set; }

	[Net]
	public bool IsNew { get; set; }

	[Net]
	public string CreatedAt { get; set; }

	[Net]
	public string UpdateAt { get; set; }

	[Net]
	public UserStats Stats { get; set; } = new();

	[Net]
	public UserPreferences Preferences { get; set; } = new();

	[Net]
	public UserCharacter Character { get; set; } = new();

	public UserDTO() {
		Id = Guid.NewGuid();
	}
}
