namespace Balea.Store.Configuration;

public class PermissionConfiguration
{
    public string Id { get; set; }
    public string Name { get; set; }
	public string Description { get; set; }

	public IList<string> Roles { get; set; } = [];
}
