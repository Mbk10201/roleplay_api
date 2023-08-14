namespace Mbk.RoleplayAPI.UI.Shared;

public class MinimapNoEntitiesHook : RenderHook
{
	public override void OnFrame( SceneCamera target )
	{

	}
}

public partial class MiniMap
{
	public static Texture RenderTexture { get; private set; }
	public static SceneCamera Camera { get; private set; }

	public static void Render( Vector3 position, Rotation rotation )
	{
		var cameraPosition = position;
		cameraPosition.z += 3500f;

		if ( Camera == null )
		{
			Camera = new SceneCamera( "Minimap" );
			Camera.World = Game.SceneWorld;
			Camera.RenderTags.Add( "world" );
		}

		Camera.FindOrCreateHook<MinimapNoEntitiesHook>();

		Camera.Ortho = true;
		Camera.OrthoHeight = 512f;
		Camera.OrthoWidth = 512f;
		Camera.Position = cameraPosition;
		Camera.Rotation = Rotation.From( new Angles( 90f, rotation.Yaw(), 0f ) );
		Camera.FieldOfView = 30f;
		Camera.BackgroundColor = Color.Transparent;
		Camera.FirstPersonViewer = null;
		Camera.ZFar = 5000f;
		Camera.ZNear = 5f;

		Graphics.RenderToTexture( Camera, GetOrCreateTexture() );
	}

	public static Texture GetOrCreateTexture()
	{
		if ( RenderTexture is not null ) return RenderTexture;
		RenderTexture = Texture.CreateRenderTarget( "Minimap", ImageFormat.RGBA8888, new Vector2( 512, 512f ) );
		return RenderTexture;
	}
}
