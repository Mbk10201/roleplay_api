using Sandbox;
using System.Linq;
using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.Systems.Computer;

/// <summary>
/// When a player is within radius of the camera we add this to their entity.
/// We remove it again when they go out of range.
/// </summary>
internal class ScreenComponent : EntityComponent<Computer>
{
	ScreenPanel ScreenPanel;

	protected override void OnActivate()
	{
		ScreenPanel = new ScreenPanel( Entity );
		Sound.FromEntity( "computer.login", Entity );
	}

	protected override void OnDeactivate()
	{
		ScreenPanel?.Delete();
		ScreenPanel = null;
		Sound.FromEntity( "computer.logout", Entity );
	}

	/// <summary>
	/// Called for every tag, while it's active
	/// </summary>
	[GameEvent.Client.Frame]
	public void FrameUpdate()
	{
		var tx = Entity.GetAttachment( "screen" ) ?? Entity.Transform;

		ScreenPanel.Transform = tx;
	}

	/// <summary>
	/// Called once per frame to manage component creation/deletion
	/// </summary>
	[GameEvent.Client.Frame]
	public static void SystemUpdate()
	{
		foreach ( var entity in Sandbox.Entity.All.OfType<Computer>() )
		{
			if ( entity.Position.Distance( Camera.Position ) > 100 )
			{
				var c = entity.Components.Get<ScreenComponent>();
				c?.Remove();
				continue;
			}

			entity.Components.GetOrCreate<ScreenComponent>();
		}
	}
}
