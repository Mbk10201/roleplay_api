namespace Mbk.RoleplayAPI.Models;

public partial class UserPreferences : BaseNetworkable
{
	[Net, JsonPropertyName( "minimap" )]
	public bool Minimap { get; set; }

	[Net, JsonPropertyName( "hud_dark_theme" )]
	public bool HudDarkTheme { get; set; }
}
