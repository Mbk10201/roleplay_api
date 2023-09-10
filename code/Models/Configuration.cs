namespace Mbk.RoleplayAPI.Models;

public partial class Configuration : BaseNetworkable
{
	[Net, Description( "The token licencing of the server." )]
	public string Token { get; set; }

	[Net, Description( "The API Url of the API." )]
	public string ApiUrl { get; set; }

	[Net, Description( "If the gamemode should start has debug mode." )]
	public bool DebugMode { get; set; }

	[Net, Description( "The respawn time." )]
	public float RespawnTime { get; set; } = 25f;

	[Net, Description( "Chat command prefix, Default !." )]
	public char ChatCommandPrefix { get; set; } = '!';

	[Net, Description( "The default alert / notification sound." )]
	public string AlertSound { get; set; } = "ui.success";

	[Net, Description("Time delay of when a player will become afk.")]
	public float AfkDelay { get; set; } = 1000f;

	[Net, Description( "Time delay of when a player will be kicked after too long AFK" )]
	public float AfkDelayKick { get; set; } = 120f;
}
