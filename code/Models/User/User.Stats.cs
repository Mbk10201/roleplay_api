namespace Mbk.RoleplayAPI.Models;

public partial class UserStats : BaseNetworkable
{
	[Net, JsonPropertyName( "tickets_closed" )]
	public int TicketsClosed { get; set; }

	[Net, JsonPropertyName( "aduty_timestamps" )]
	public int AdutyTimestamp { get; set; }
}
