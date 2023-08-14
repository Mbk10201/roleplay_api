using Editor;

namespace Mbk.RoleplayAPI.Entities.Hammer;

[Library( "rp_scene_attachment" )]
[Display( Name = "rp_scene_attachment" ), Category( "Scene" ), Icon( "ads_click" )]
[HammerEntity]
public partial class SceneAttachment : Entity
{
	[Property( Title = "Area Name" )]
	public string AreaName { get; set; }

	public SceneAttachment()
	{
		Log.Info( "SceneAttachment" );
	}

	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;
	}
}
