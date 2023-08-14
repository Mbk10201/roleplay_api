namespace Mbk.RoleplayAPI.Jobs;

[Serializable]
public class Grade : BaseNetworkable
{
	public int Id { get; set; }
	public string Name { get; set; }
	public int Salary { get; set; }
}
