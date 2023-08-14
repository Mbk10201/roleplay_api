using System;

namespace Mbk.RoleplayAPI.Inventory;

public class ResourceItem<A,T> : InventoryItem, IResourceItem where A : ItemResource where T : ResourceItem<A,T>
{
	/// <summary>
	/// The name of the item.
	/// </summary>
	public override string Name => Resource?.ItemName ?? string.Empty;

	/// <summary>
	/// The description of the item.
	/// </summary>
	public override string Description => Resource?.Description ?? string.Empty;

	/// <summary>
	/// The world model to use when the item is dropped.
	/// </summary>
	public override string WorldModel => Resource?.WorldModel ?? string.Empty;

	/// <summary>
	/// The unique id of the item.
	/// </summary>
	public override string UniqueId => Resource?.UniqueId ?? string.Empty;

	/// <summary>
	/// The icon to use for the item.
	/// </summary>
	public override string Icon => Resource?.Icon ?? string.Empty;

	public override ushort MaxStackSize => Resource?.MaxStackSize ?? 1;
	
	/// <summary>
	/// The item resource asset data.
	/// </summary>
	public A Resource { get; protected set; }
	
	ItemResource IResourceItem.Resource => Resource;

	public ResourceItem()
	{
		Sellable = Resource?.Sellable ?? true;
		CanBeTraded = Resource?.CanBeTraded ?? true;
		Enabled = Resource?.Enabled ?? true;
		Price = Resource?.Price ?? 500;
		Weight = Resource?.Weight ?? 5f;
		//Rarity = Resource?.Rarity ?? ItemRarity.Common;
		int random = Game.Random.Int( Enum.GetValues(typeof(ItemRarity)).Cast<int>().Max() );
		Rarity = (ItemRarity)random;
		Type = Resource?.Type ?? ItemType.Object;
		RequiredLevel = Resource?.RequiredLevel ?? 1;
		Durability = Resource?.Durability ?? 0f;
		CooldownTime = Resource?.CooldownTime ?? 0f;
		JobsWhitelist = Resource?.JobsWhitelist ?? null;
		CraftingRequirements = Resource?.CraftingRequirements ?? null;
		IsUnique = Resource?.IsUnique ?? false;
		UsageTagsRestrictions = Resource?.UsageTagsRestrictions ?? null;
		CanUseInPVPZone = Resource?.CanUseInPVPZone ?? true;
	}

	/// <summary>
	/// Load data from an item resource asset.
	/// </summary>
	/// <param name="resource"></param>
	public void LoadResource( ItemResource resource )
	{
		if ( resource is not A )
		{
			Log.Error( $"Unable to load resource for { GetType().Name }. Resource is not of type: {typeof( A ).Name}!" );
			return;
		}

		InternalTags = new HashSet<string>( resource.Tags, StringComparer.OrdinalIgnoreCase );
		Resource = resource as A;

		BuildTags( InternalTags );
	}
}
