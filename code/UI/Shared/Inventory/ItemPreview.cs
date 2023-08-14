using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.UI.Shared.Inventory;

public class ItemPreview : Panel
{
	private ScenePanel ScenePanel;
	private SceneWorld SceneWorld;
	public bool Rotate { get; set; } = false;

	public ItemPreview( string model )
	{
		Build( model );
	}

	private void Build(string model)
	{
		SceneWorld?.Delete();
		ScenePanel?.Delete();

		SceneWorld = new SceneWorld();
		ScenePanel = Add.ScenePanel( SceneWorld, Vector3.Backward * 100 + Vector3.Up * 30, Rotation.Identity, 10 );
		ScenePanel.Camera.Rotation = Rotation.FromPitch( 15 );
		ScenePanel.Camera.ZFar = 512;
		ScenePanel.Camera.ZNear = 5;

		ScenePanel.Style.Width = Length.Percent( 100 );
		ScenePanel.Style.Height = Length.Percent( 100 );

		_ = new SceneModel( SceneWorld, model, Transform.Zero.WithRotation( Rotation.FromYaw( 180 ) ) );

		//_ = new SceneLight( SceneWorld, Vector3.Up * 100, 200f, Color.White * 5 );
		_ = new SceneLight( SceneWorld, Vector3.Backward * 100 + Vector3.Up * 50f, 200f, Color.White * 5 ).ShadowsEnabled = false;
	}

	public void ResetPosition()
	{
		if ( !SceneWorld.IsValid() ) return;

		foreach ( var obj in SceneWorld.SceneObjects )
		{
			if ( obj is not SceneModel m ) continue;
			m.Update( RealTime.Delta );

			obj.Rotation = Rotation.From( 0, 180, 0 );
		}
	}

	public override void Tick()
	{
		base.Tick();

		if ( !SceneWorld.IsValid() ) return;

		foreach ( var obj in SceneWorld.SceneObjects )
		{
			if ( obj is not SceneModel m ) continue;
			m.Update( RealTime.Delta );

			if( Rotate )
			{
				obj.Rotation = Rotation.Lerp( obj.Rotation, obj.Rotation * Rotation.FromYaw( 400f ), 1f * RealTime.Delta );
			}
		}
	}

}
