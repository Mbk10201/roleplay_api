namespace Mbk.RoleplayAPI.UI.Shared.AlertSystem;

//
// Summary:
//     Indicates that this panel can be created as a game alert panel. If we find a Panel
//     in a hud or library assemblies that implements this then we'll try to
//     use it as the game's alert panel rather than using the default.
public interface IAlertPanel
{
	void Add( AlertBuilder item );
}
