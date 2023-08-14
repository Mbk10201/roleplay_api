using Mbk.RoleplayAPI.Inventory;
using System.IO;

namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	[ConCmd.Server( "fsk.item.throw" )]
	private static void ThrowItemCmd( ulong itemId, string directionCsv, bool splitStack )
	{
		if ( ConsoleSystem.Caller.Pawn is not RoleplayPlayer player )
			return;

		var split = directionCsv.Split( ',' );
		var direction = new Vector3( split[0].ToFloat(), split[1].ToFloat(), 0f );
		var item = InventorySystem.FindInstance( itemId );

		if ( item.IsValid() )
		{
			if ( splitStack && item.StackSize > 1 )
			{
				var splitAmount = item.StackSize / 2;
				item.StackSize -= (ushort)splitAmount;

				item = InventorySystem.DuplicateItem( item );
				item.StackSize = (ushort)splitAmount;
			}

			var entity = InventorySystem.CreateItemEntity( item );
			entity.Position = player.EyePosition + direction * 10f;
			entity.ApplyLocalImpulse( direction * 300f + Vector3.Down * 10f );
		}
	}

	public static void ThrowItem( IInventoryItem item, Vector3 direction, bool splitStack = false )
	{
		if ( !item.IsValid() ) return;
		var csv = $"{direction.x},{direction.y}";
		ThrowItemCmd( item.ItemId, csv, splitStack );
	}

	private void OnBackpackItemGiven( ushort slot, IInventoryItem instance )
	{

	}

	private void OnBackpackItemTaken( ushort slot, IInventoryItem instance )
	{

	}

	private void GiveInitialItems()
	{
		Backpack.Give( InventorySystem.CreateItem( "medkit" ) );
		Backpack.Give( InventorySystem.CreateItem( "medkit" ) );
		Backpack.Give( InventorySystem.CreateItem( "medkit" ) );
		Backpack.Give( InventorySystem.CreateItem( "medical_pills" ) );
		Backpack.Give( InventorySystem.CreateItem( "medical_pills" ) );
	}

	private void CreateInventories()
	{
		var backpack = new InventoryContainer();
		backpack.SetSlotLimit( 24 );
		backpack.SetEntity( this );
		backpack.AddConnection( Client );
		backpack.ItemTaken += OnBackpackItemTaken;
		backpack.ItemGiven += OnBackpackItemGiven;
		InventorySystem.Register( backpack );

		InternalBackpack = new NetInventoryContainer( backpack );
	}

}
