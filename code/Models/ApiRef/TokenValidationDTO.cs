namespace Mbk.RoleplayAPI.Models.ApiRef;

public partial class TokenValidationDTO
{
	public string token { get; set; }
	public bool enabled { get; set; }
	public string reason { get; set; }
}
