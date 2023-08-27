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
			Name = "Commandant",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 3,
			Name = "Lieutenant",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 4,
			Name = "Sergent II",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 5,
			Name = "Sergent I",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 6,
			Name = "SLO",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 7,
			Name = "Officier III",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 8,
			Name = "Officier II",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 9,
			Name = "Officier I",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 10,
			Name = "Agent",
			Salary = 2500
		} );
	}
}
