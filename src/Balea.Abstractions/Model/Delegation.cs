namespace Balea
{
    public class Delegation
    {
    	public string Id { get; set; }
    	public Subject Who { get; set; }
    	public Subject Whom { get; set; }
    	public DateTime From { get; set; }
    	public DateTime To { get; set; }
        public bool Enabled { get; set; } = true;

        public bool Active => Enabled && From <= DateTime.UtcNow && To >= DateTime.UtcNow;
    }
}
