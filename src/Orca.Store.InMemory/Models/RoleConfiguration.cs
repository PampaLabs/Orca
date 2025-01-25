namespace Orca.Store.Configuration;

internal class RoleConfiguration
{
    public string Name { get; set; }
	public string Description { get; set; }
	public bool Enabled { get; set; } = true;
	public HashSet<string> Mappings { get; set; } = [];

    public HashSet<string> Permissions { get; set; } = [];
    public HashSet<string> Subjects { get; set; } = [];
}
