namespace Mbk.RoleplayAPI.Database.DTO;

public partial class UserCharacter : BaseNetworkable
{
	[Net, JsonPropertyName( "gender" )]
	public int Gender { get; set; } = 0;
}
