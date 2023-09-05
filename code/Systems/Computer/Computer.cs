using Editor;

namespace Mbk.RoleplayAPI.Systems.Computer;

[Library( "Computer" )]
[Display( Name = "Computer" ), Category( "Electronics" ), Icon( "computer" )]
[HammerEntity]
public partial class Computer : ModelEntity
{
	public override void Spawn()
	{
		Tags.Add( "solid" );

		Model = Cloud.Model( "https://asset.party/eurorp/monitor" );

		Transmit = TransmitType.Always;
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		base.Spawn();
	}

	[ClientRpc]
	public static void OpenWindow(IWindow window)
	{
		if ( Navigator.Instance == null )
			return;

		_ = new Window(window);
	}
}
