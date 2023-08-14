using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.UI.Scenes;

internal class CitizenPanel : Panel
{
	private RoleplayPlayer player;
	private ScenePanel ScenePanel;
	private SceneWorld SceneWorld;
	private SceneModel Citizen;
	private SceneModel Head;

	public CitizenPanel( )
	{
		Build();
	}

	public CitizenPanel( RoleplayPlayer pl )
	{
		Build();
	}

	[Event.Hotload]
	private void Build()
	{
		//Assert.True( player.IsValid() );

		SceneWorld?.Delete();
		ScenePanel?.Delete();

		SceneWorld = new SceneWorld();
		ScenePanel = Add.ScenePanel( SceneWorld, Vector3.Backward * 100 + Vector3.Up * 50 + Vector3.Left * 25, Rotation.Identity, 60 );
		//ScenePanel.Camera.Rotation = Rotation.FromPitch(5 );
		ScenePanel.Camera.ZFar = 512;
		ScenePanel.Camera.ZNear = 5;

		ScenePanel.Style.Width = Length.Percent( 100 );
		ScenePanel.Style.Height = Length.Percent( 100 );

		Citizen = new SceneModel( SceneWorld, "models/humans/male.vmdl", Transform.Zero.WithRotation( Rotation.FromYaw( 160 )));
		Head = new SceneModel( SceneWorld, "models/humans/heads/adam/adam.vmdl", Citizen.GetBoneWorldTransform( "head" ) );

		Citizen.AddChild( "head", Head );
		Head.SetBodyGroup( "Hair", 2 );

		_ = new SceneLight( SceneWorld, Vector3.Up * 100, 200f, Color.White * 5 );
		_ = new SceneLight( SceneWorld, Vector3.Backward * 100 + Vector3.Up * 50f, 200f, Color.White * 5 ).ShadowsEnabled = false;

		//Dress( citizen, player.Avatar );
	}

	public override void Tick()
	{
		base.Tick();

		if ( !SceneWorld.IsValid() ) return;

		foreach ( var obj in SceneWorld.SceneObjects )
		{
			if ( obj is not SceneModel m ) continue;
			m.Update( RealTime.Delta );
		}
	}

	/*private void Dress( SceneModel m, string json )
	{
		var container = new ClothingContainer();
		container.Deserialize( json );
		container.DressSceneObject( m );
	}*/

}
