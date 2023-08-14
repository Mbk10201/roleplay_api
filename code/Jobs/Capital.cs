namespace Mbk.RoleplayAPI.Jobs;

public class Capital : BaseNetworkable
{
	public class Transfert
	{
		public int Amount { get; set; }
		public int OldCapital { get; set; }
		public DateTime date { get; set; }
	}
	
	public int Value { get; set; }
	public List<Transfert> Transferts { get; set; }

	public void AddTransfert(Transfert transfert) => Transferts.Add(transfert);
}
