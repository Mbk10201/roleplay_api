using Mbk.RoleplayAPI.Database.DTO;
using Mbk.RoleplayAPI.Inventory;

namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	[Net]
	public UserDTO Data { get; set; } = null;

	[Net]
	public string CurrentZone { get; set; } = "N/A";

	[Net]
	public float Armor { get; set; }

	[Net]
	public float MaxArmor { get; set; } = 100f;

	[Net, Predicted]
	public bool ThirdPerson { get; set; }

	[Net]
	public bool IsNew { get; set; } = false;

	[Net]
	public Entity LastAimEntity { get; set; }

	[Net] 
	private NetInventoryContainer InternalBackpack { get; set; }

	[Net]
	public bool IsAFK { get; set; } = false;

	public bool IsPressingButton { get; set; }
	public string IsPressingButtonName { get; set; }

	private DamageInfo LastDamage;

	public InventoryContainer Backpack => InternalBackpack.Value;

	[Net]
	public TimeUntil TimeUntilRespawn { get; set; }

	/*			TIMESINCE			*/
	public TimeSince TimeSinceLastDamage { get; set; }
	public TimeSince TimeSinceDamage { get; set; } = 1.0f;
	public TimeSince TimeSinceDropped { get; set; } = 1.0f;
	public TimeSince TimeSinceLastFootstep { get; set; }
	public TimeSince TimeSinceAFK { get; set; }
	/*			TIMESINCE			*/

	public WorldInput WorldInput = new();
	public List<MapMarker> Markers { get; private set; } = new List<MapMarker>();

	private TimeSince TimeSinceBackpackOpen { get; set; }
	private bool IsBackpackToggleMode { get; set; }
}
