using Mbk.RoleplayAPI.Database.DTO;
using Mbk.RoleplayAPI.Inventory;

namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	[Net]
	public UserDTO Data { get; set; }

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
	public TimeSince TimeSinceDied { get; set; }

	[Net]
	public Entity LastAimEntity { get; set; }

	[Net] 
	private NetInventoryContainer InternalBackpack { get; set; }

	public InventoryContainer Backpack => InternalBackpack.Value;

	/*			TIMESINCE			*/
	public TimeSince TimeSinceLastDamage { get; set; }
	public TimeSince TimeSinceDamage { get; set; } = 1.0f;
	public TimeSince TimeSinceDropped { get; set; } = 1.0f;
	public TimeSince TimeSinceLastFootstep { get; set; }
	/*			TIMESINCE			*/

	public WorldInput WorldInput = new();
	public List<MapMarker> Markers { get; private set; } = new List<MapMarker>();

	private TimeSince TimeSinceBackpackOpen { get; set; }
	private bool IsBackpackToggleMode { get; set; }
}
