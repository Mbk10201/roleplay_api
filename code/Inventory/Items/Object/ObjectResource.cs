namespace Mbk.RoleplayAPI.Inventory;

[GameResource( "Object", "object", "Describe and configure a game object item for the inventory system", Category = "roleplay", Icon = "extension", IconBgColor = "white" )]
[ItemClass( typeof( ObjectItem ) )]
public partial class ObjectResource : ItemResource
{
}
