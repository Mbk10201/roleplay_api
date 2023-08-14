namespace Mbk.RoleplayAPI.Inventory;

[GameResource( "Weapon", "weapon", "Describe and configure a game object item for the inventory system", Category = "roleplay", Icon = "extension", IconBgColor = "white" )]
[ItemClass( typeof( WeaponItem ) )]
public partial class WeaponResource : ItemResource
{
	[Property]
	public string WorldModelPath { get; set; }

	[Property]
	public string ViewModelPath { get; set; }

	[Property]
	public eAmmoType AmmoType { get; set; } = eAmmoType.None;

	[Property]
	public int DefaultAmmo { get; set; } = 0;

	[Property]
	public int ClipSize { get; set; } = 0;

	[Property]
	public float ReloadTime { get; set; } = 0f;

	[Property]
	public float PrimaryRate { get; set; } = 15f;

	[Property, ResourceType( "vpcf" )]
	public string ShootParticle { get; set; } = "";
}
