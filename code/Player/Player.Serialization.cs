using Mbk.RoleplayAPI.Inventory;
using System.IO;

namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	public virtual void SerializeState( BinaryWriter writer )
	{
		writer.Write( Backpack );
	}

	public virtual void DeserializeState( BinaryReader reader )
	{
		var backpack = reader.ReadInventoryContainer( Backpack );
		InternalBackpack = new NetInventoryContainer( backpack );
	}
}
