using Mbk.RoleplayAPI.World;

namespace Mbk.RoleplayAPI.Jobs.List;

internal class JobHospital : Job
{
	public override string Identifier => "hospital";

	public JobHospital()
	{
		Name = "Hospital";
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
			Name = "Superviseur",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 3,
			Name = "Coordinateur",
			Salary = 2500
		} );

		Grades.Add( new Grade()
		{
			Id = 4,
			Name = "Paramedic",
			Salary = 2500
		} );
	}
}
