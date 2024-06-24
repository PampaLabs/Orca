namespace Balea.Store.Configuration;

public class DelegationConfiguration
{
	public string Id { get; set; }
	public string Who { get; set; }
	public string Whom { get; set; }
	public DateTime From { get; set; }
	public DateTime To { get; set; }
    public bool Enabled { get; set; } = true;

    public bool Active => Enabled && From <= DateTime.UtcNow && To >= DateTime.UtcNow;
}
