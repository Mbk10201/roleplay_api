using Sandbox;

namespace Mbk.RoleplayAPI.Systems.Computer;

public interface IComputerApp : IWindow
{
	/// <summary>
	/// The name of this app.
	/// (Required)
	/// </summary>
	string AppName { get; }

	/// <summary>
	/// Can this app be opened more than once (Ex: Google)
	/// (Required)
	/// </summary>
	bool AppMultipleInstance { get; }

	/// <summary>
	/// The jobs that has access to this app.
	/// /// Use like : new string[]{"police", "hospital"}
	/// (Optional)
	/// If optional leave default value as ""
	/// </summary>
	string[] AppJobAccess { get; }

	/// <summary>
	/// The roles id that has access to this app.
	/// Use like : new long[]{40000}
	/// (Optional)
	/// If optional leave default value as -1
	/// </summary>
	long[] AppRoleId { get; }
}
