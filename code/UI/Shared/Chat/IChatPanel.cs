namespace Mbk.RoleplayAPI.UI.Shared.Chat;

//
// Summary:
//     Indicates that this panel can be created as a game chat panel. If we find a Panel
//     in a hud or library assemblies that implements this then we'll try to
//     use it as the game's chats panel rather than using the default.
public interface IChatPanel
{
	bool IsOpen { get; set; }

	void AddMessage( string name, string message, string classes);
	void Open();
	void Close();
}
