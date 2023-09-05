using Sandbox;

namespace Mbk.RoleplayAPI.Systems.Computer;

public interface IComputerApp : IWindow
{
	/// <summary>
	/// The name of this app.
	/// </summary>
	string AppName { get; }
}
