namespace Mbk.RoleplayAPI.Jobs.List;

internal class JobDefault : Job
{
	public override string Identifier => "default";

	public JobDefault()
	{
		Name = "Jobless";
		Grades.Add( new()
		{
			Id = 1,
			Salary = 500,
			Name = "Unemployed"
		} );
	}
}
