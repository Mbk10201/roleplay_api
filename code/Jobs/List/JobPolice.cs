using Mbk.RoleplayAPI.World;

namespace Mbk.RoleplayAPI.Jobs.List;

internal class JobPolice : Job
{
	public override string Identifier => "police";

	public JobPolice()
	{
		Name = "Police";
		CanSell = true;

		Grades.Add( new Grade()
		{
			Id = 1,
			Name = "Chef",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 2,
			Name = "Co chef",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 3,
			Name = "Agent",
			Salary = 2500
		} );
	}
}
