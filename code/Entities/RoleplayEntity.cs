namespace Mbk.RoleplayAPI.Entities;

public class RoleplayEntity : Prop
{
	public virtual bool HaveTag => false;
	public virtual bool CanBePorted => false;
	public virtual string DisplayName => "N/A";
	public string CurrentZone { get; set; } = "N/A";

	public override void Spawn()
	{
		base.Spawn();

		Tags.Add( "roleplayentity" ); 
	}
}
