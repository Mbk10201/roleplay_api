namespace Mbk.RoleplayAPI.Models;

public partial class Configuration : BaseNetworkable
{
	[Net]
	public string Token { get; set; }

	[Net]
	public string ApiUrl { get; set; }

	[Net]
	public bool DebugMode { get; set; }

	[Net]
	public float RespawnTime { get; set; } = 2f;

	[Net]
	public char ChatCommandPrefix { get; set; } = '!';

	[Net]
	public string AlertSound { get; set; } = "ui.success";
}
