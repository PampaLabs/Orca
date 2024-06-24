namespace Balea
{
    public class RoleFilter
    {
        public string Name { get; set; }
    	public string Description { get; set; }
    	public bool? Enabled { get; set; }

    	public IList<string> Mappings { get; set; }
        public IList<string> Subjects { get; set; }
    }
}
