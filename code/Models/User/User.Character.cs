namespace Mbk.RoleplayAPI.Models;

public partial class UserCharacter : BaseNetworkable
{
	[Net, JsonPropertyName( "gender" )]
	public int Gender { get; set; } = 0;
}
