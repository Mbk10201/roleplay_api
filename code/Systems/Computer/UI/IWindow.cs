using Sandbox;

namespace Mbk.RoleplayAPI.Systems.Computer;

public interface IWindow
{
	/// <summary>
	/// This window name
	/// </summary>
	string WindowName { get; }

	/// <summary>
	/// Should this window be opened fullscreen (hide bar) ?
	/// </summary>
	bool Fullscreen { get; }

	/// <summary>
	/// The icon of the window
	/// https://icones.js.org
	/// </summary>
	string Icon { get; }

	/// <summary>
	/// The icon color (HEX, RGB, RGBA)
	/// Optional
	/// </summary>
	string IconColor { get; }
}
