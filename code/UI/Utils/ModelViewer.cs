namespace Mbk.RoleplayAPI.UI.Utils;

public class ModelViewer : ScenePanel
{
	public SceneModel Model { get; set; }
	public List<SceneModel> Models = new List<SceneModel>();

	/// <summary>
	/// Scale the model's animation time by this amount
	/// </summary>
	public float TimeScale { get; set; } = 1.0f;


	public CameraMode CameraController { get; set; }

	public ModelViewer()
	{
		World?.Delete();
		World = new();
	}

	public override void OnDeleted()
	{
		base.OnDeleted();

		Model?.Delete();
		Model = null;

		World?.Delete();
		World = null;
	}

	public override void Tick()
	{
		base.Tick();

		if ( !IsVisible ) return;

		foreach ( var model in Models )
		{
			model.Update( Time.Delta * TimeScale );
		}

		CameraController?.Update( this );
	}

	public override bool OnTemplateElement( INode element )
	{

		foreach ( var child in element.Children )
		{
			if ( child.Name.ToLower() == "light" )
			{
				var pos = child.GetAttribute<Vector3>( "position" );
				var radius = child.GetAttribute<float>( "radius", 200.0f );
				var color = child.GetAttribute<Color>( "color", Color.White );
				var light = new SceneLight( World, pos, radius, color );
				light.ShadowsEnabled = true;
				light.ShadowTextureResolution = child.GetAttribute<int>( "shadowres", 0 );
				light.QuadraticAttenuation = child.GetAttribute<float>( "qatten", 1.0f );
				light.LinearAttenuation = child.GetAttribute<float>( "latten", 0.0f );
				light.Radius = radius;
			}

			if ( child.Name.ToLower() == "model" )
			{
				var modelName = child.GetAttribute<string>( "src" );
				var pos = child.GetAttribute<Vector3>( "position" );
				var model = new SceneModel( World, modelName, new Transform( pos ) );
			}
		}

		return true;
	}

	public SceneModel AddModel( Model model, Transform transform )
	{
		var o = new SceneModel( World, model, transform );
		Models.Add( o );
		return o;
	}

	public void RemoveModel( SceneModel model )
	{
		Models.Remove( model );
		model?.Delete();
	}

	public void AddModels( IEnumerable<SceneModel> models )
	{
		Models.AddRange( models );
	}

	internal SceneModel SetModel( string modelName )
	{
		if ( Model != null )
		{
			Models.Remove( Model );
			Model.Delete();
			Model = null;
		}

		var model = Sandbox.Model.Load( modelName );
		Model = new SceneModel( World, model, Transform.Zero );

		Models.Add( Model );
		Model.Update( 0.1f );

		return Model;
	}

	public class CameraMode
	{
		public virtual void Update( ModelViewer mv )
		{

		}
	}

	public class Orbit : CameraMode
	{
		public Vector3 Center;
		public Vector3 Offset;
		public Angles Angles;
		public float Distance;

		public Vector2 PitchLimit = new Vector2( -90, 90 );
		public Vector2 YawLimit = new Vector2( -360, 360 );

		public Angles HomeAngles;
		public Vector3 SpinVelocity;

		public Orbit( Vector3 center, Vector3 normal, float distance )
		{
			Center = center;
			HomeAngles = Rotation.LookAt( normal.Normal ).Angles();
			Angles = HomeAngles;
			Distance = distance;
		}

		public override void Update( ModelViewer mv )
		{
			if ( mv.HasActive )
			{
				var move = Mouse.Delta;

				SpinVelocity.x = move.y * -1.0f;
				SpinVelocity.y = move.x * 3.0f;

				Angles.pitch += SpinVelocity.x * 0.1f;
				Angles.yaw += SpinVelocity.y * 0.1f;
			}
			else
			{
				SpinVelocity = SpinVelocity.LerpTo( 0, Time.Delta * 2.0f );
				Angles.pitch += SpinVelocity.x * Time.Delta;
				Angles.yaw += SpinVelocity.y * Time.Delta;
			}


			Angles.roll = 0;

			Angles = Angles.Normal;

			Angles.pitch = Angles.pitch.Clamp( PitchLimit.x, PitchLimit.y );
			Angles.yaw = Angles.yaw.Clamp( YawLimit.x, YawLimit.y );


			mv.Camera.Rotation = Rotation.From( Angles );
			mv.Camera.Position = Center + (mv.Camera.Rotation.Backward * Distance) + Offset;
		}
	}
}

