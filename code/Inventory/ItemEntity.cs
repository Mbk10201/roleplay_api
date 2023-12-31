﻿namespace Mbk.RoleplayAPI.Inventory;

/// <summary>
/// The base item entity. It can be extended and the <see cref="ItemEntityAttribute"/> attribute could be added to the extended class.
/// </summary>
public partial class ItemEntity : ModelEntity
{
	public TimeUntil TimeUntilCanPickup { get; set; }
	public TimeSince TimeSinceSpawned { get; set; }

	[Net] private NetInventoryItem InternalItem { get; set; }
	public IInventoryItem Item => InternalItem.Value;

	public void SetItem( IInventoryItem item )
	{
		var worldModel = !string.IsNullOrEmpty( item.WorldModel ) ? item.WorldModel : "models/sbox_props/burger_box/burger_box.vmdl";

		if ( !string.IsNullOrEmpty( worldModel ) )
		{
			SetModel( worldModel );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		InternalItem = new NetInventoryItem( item );
		item.SetWorldEntity( this );
	}

	public IInventoryItem Take()
	{
		if ( IsValid && Item.IsValid() )
		{
			var item = Item;

			item.ClearWorldEntity();
			InternalItem = null;
			Delete();

			return item;
		}

		return null;
	}

	public virtual void Reset()
	{
		Delete();
	}

	public override void Spawn()
	{
		TimeUntilCanPickup = 1f;
		TimeSinceSpawned = 0f;
		Transmit = TransmitType.Always;

		Tags.Add( "hover", "solid", "passplayers" );

		base.Spawn();
	}
}

