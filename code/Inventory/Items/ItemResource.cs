namespace Mbk.RoleplayAPI.Inventory;

public class ItemResource : GameResource
{
	/// <summary>
	/// The item name.
	/// </summary>
	[Property]
	public string ItemName { get; set; }

	/// <summary>
	/// The unique id of the item.
	/// </summary>
	[Property]
	public string UniqueId { get; set; }

	/// <summary>
	/// The item description.
	/// </summary>
	[Property]
	public string Description { get; set; }

	/// <summary>
	/// The icon to use for the item.
	/// </summary>
	[Property]
	public string Icon { get; set; }

	/// <summary>
	/// The world model to use when the item is dropped.
	/// </summary>
	[Property, ResourceType( "vmdl" )]
	public string WorldModel { get; set; } = "models/citizen_props/beachball.vmdl_c";

	/// <summary>
	/// The maximum amount of this item that can be stacked.
	/// </summary>
	[Property]
	public ushort MaxStackSize { get; set; } = 1;
	
	/// <summary>
	/// If this item can be sold
	/// </summary>
	[Property]
	public bool Sellable { get; set; } = true;

	/// <summary>
	/// If this item can be traded between players.
	/// </summary>
	[Property]
	public bool CanBeTraded { get; set; } = true;

	/// <summary>
	/// If this item is enabled.
	/// Its used for displaying it on containers.
	/// </summary>
	[Property]
	public bool Enabled { get; set; } = true;

	/// <summary>
	/// The price of the item.
	/// </summary>
	[Property]
	public int Price { get; set; } = 500;

	/// <summary>
	/// The weight of the item, in KG preferably :D
	/// </summary>
	[Property]
	public float Weight { get; set; } = 5f;

	/// <summary>
	/// The item rarity.
	/// <see cref="ItemRarity"/>.
	/// </summary>
	[Property]
	public ItemRarity Rarity { get; set; } = ItemRarity.Common;

	/// <summary>
	/// The item type
	/// <see cref="ItemType"/>.
	/// </summary>
	[Property]
	public ItemType Type { get; set; } = ItemType.Object;

	/// <summary>
	/// The required level to be able to get and use this item.
	/// </summary>
	[Property]
	public int RequiredLevel { get; set; } = 1;

	/// <summary>
	/// The durability / effect of this item.
	/// This can be optional and depends on the item.
	/// </summary>
	[Property]
	public float Durability { get; set; }

	/// <summary>
	/// Cooldown time before be able to re-use this item.
	/// </summary>
	[Property]
    public float CooldownTime { get; set; }

	/// <summary>
	/// The jobs id's that can sell this item.
	/// </summary>
	[Property]
    public List<string> JobsWhitelist { get; set; }

	/// <summary>
	/// The items id and the amount required to be able to craft this item.
	/// </summary>
	[Property]
    public Dictionary<string, int> CraftingRequirements { get; set; }

	/// <summary>
	/// If this item is unique in the game instance.
	/// Means very very rare !
	/// </summary>
	[Property]
    public bool IsUnique { get; set; }

	/// <summary>
	/// Restrict the usage of the item if players contains the marked tags of the list.
	/// </summary>
	[Property]
    public List<string> UsageTagsRestrictions { get; set; }

	/// <summary>
	/// If this item can be used in the pvp zone.
	/// </summary>
	[Property]
	public bool CanUseInPVPZone { get; set; } = true;

	/// <summary>
	/// A list of tags that describe the item.
	/// </summary>
	[Property]
	public List<string> Tags { get; set; } = new();

	protected override void PostLoad()
	{
		if ( Game.IsServer || Game.IsClient )
		{
			InventorySystem.ReloadDefinitions();
		}

		base.PostLoad();
	}

	protected override void PostReload()
	{
		if ( Game.IsServer || Game.IsClient )
		{
			InventorySystem.ReloadDefinitions();
		}
		
		base.PostReload();
	}
}
