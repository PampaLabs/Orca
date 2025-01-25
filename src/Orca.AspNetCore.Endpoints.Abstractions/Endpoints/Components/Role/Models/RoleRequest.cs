namespace Orca.AspNetCore.Endpoints;

public class RoleRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; } = true;
    public IList<string> Mappings { get; set; }
}
