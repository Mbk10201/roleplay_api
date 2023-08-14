namespace Mbk.RoleplayAPI.Jobs;

public partial class JobSystem 
{
	public const string OnGiveSalary = "ongivesalary";

	public class OnGiveSalaryAttribute : EventAttribute
	{
		public OnGiveSalaryAttribute() : base( OnGiveSalary ) { }
	}
	
	public const string OnTakeService = "ontakeservice";

	public class OnTakeServiceAttribute : EventAttribute
	{
		public OnTakeServiceAttribute() : base( OnTakeService ) { }
	}
}
