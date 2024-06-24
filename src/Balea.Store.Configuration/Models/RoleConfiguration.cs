namespace Balea.Store.Configuration;

public class RoleConfiguration
{
    public string Id { get; set; }
    public string Name { get; set; }
	public string Description { get; set; }
	public bool Enabled { get; set; } = true;
	public IList<string> Mappings { get; set; } = [];
	public IList<string> Subjects { get; set; } = [];
}
