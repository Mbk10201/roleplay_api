using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.World;

namespace Mbk.RoleplayAPI.Jobs;

public abstract partial class Job : BaseNetworkable
{
	public virtual string Identifier => string.Empty;
	public string Name { get; set; }
	public bool CanSell { get; set; } = false;
	public bool IsGovernment { get; set; } = false;
	public bool IsEnabled { get; set; } = true;
	public  Color Color => new Color( 255, 255, 255 );
	public int RequiredLevel { get; set; } = 1;
	public float SalaryInterval { get; set; } = WeatherSystem.OneDayInSecond;

	[Net] public Capital Capital { get; set; }
	[Net] public IList<Grade> Grades { get; set; }
	[Net] public IList<Vector3> Spawns { get; set; }

	public IList<RoleplayPlayer> GetMembers {
		get {
			return Entity.All.OfType<RoleplayPlayer>().Where( x => x.Data.JobID == Identifier ).ToList();
		}
	}

	public Job()
	{
		Capital = new Capital();
		Grades = new List<Grade>();
		Spawns = new List<Vector3>();
	}

	public virtual void TakeService()
	{
		// Discord Webhook send info in the job channel
		Event.Run( JobSystem.OnTakeService );
	}

	public virtual void Hire( RoleplayPlayer player )
	{

	}
}
