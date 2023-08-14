namespace Mbk.RoleplayAPI.Jobs;

public partial class JobSystem
{
	public static string GetJobName( string identifier ) => Instance.Jobs.Single( job => job.Identifier == identifier ).Name;
	
	public static Color GetJobColor( string identifier ) => Instance.Jobs.Single( job => job.Identifier == identifier ).Color;
	
	public static bool CanJobSell( string identifier ) => Instance.Jobs.Single( job => job.Identifier == identifier ).CanSell;
	
	public static Capital GetJobCapital( string identifier ) => Instance.Jobs.Single( job => job.Identifier == identifier ).Capital;
	
	public static string GetGradeName( string identifier, int grade ) => Instance.Jobs.Single( job => job.Identifier == identifier ).Grades.Single( g => g.Id == grade ).Name;
	public static int GetGradeSalary( string identifier, int grade ) => Instance.Jobs.Single( job => job.Identifier == identifier ).Grades.Single( g => g.Id == grade ).Salary;
}
