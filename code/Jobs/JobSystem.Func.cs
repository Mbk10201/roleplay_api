namespace Mbk.RoleplayAPI.Jobs;

public partial class JobSystem
{
	public static string GetJobName( string identifier )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		return (job != null ? job.Name : "Unknown");
	}

	public static Color GetJobColor( string identifier )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		return (job != null ? job.Color : Color.White);
	}
	
	public static bool CanJobSell( string identifier )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		return (job != null ? job.CanSell : false);
	}
	
	public static Capital GetJobCapital( string identifier )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		return (job != null ? job.Capital : null);
	}
	
	public static string GetGradeName( string identifier, int gradeid )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		Grade grade = null;

		if ( job != null )
		{
			grade = job.Grades.SingleOrDefault( g => g.Id == gradeid );
		}

		return (grade != null ? grade.Name : "Unknown");
	}

	public static int GetGradeSalary( string identifier, int gradeid )
	{
		var job = Instance.Jobs.SingleOrDefault( job => job.Identifier == identifier );
		Grade grade = null;

		if ( job != null )
		{
			grade = job.Grades.SingleOrDefault( g => g.Id == gradeid );
		}

		return (grade != null ? grade.Salary : 0);
	}
}
