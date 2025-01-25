namespace Orca
{
    public class OrcaWebHost
        {
    	public OrcaEvents Events { get; set; } = new();
    	public List<string> Schemes { get; set; } = new();
    }
}
