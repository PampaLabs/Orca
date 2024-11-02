namespace Balea.Store.EntityFrameworkCore.Entities;

public class DelegationEntity
{
	public string Id { get; set; }
	public string Who { get; set; }
	public string Whom { get; set; }
	public DateTime From { get; set; }
	public DateTime To { get; set; }
	public bool Enabled { get; set; } = true;
}
