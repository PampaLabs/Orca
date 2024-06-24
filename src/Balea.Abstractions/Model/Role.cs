namespace Balea
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    	public string Description { get; set; }
    	public bool Enabled { get; set; } = true;
    	public IList<string> Mappings { get; set; }
    }
}
