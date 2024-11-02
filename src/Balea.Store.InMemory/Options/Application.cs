namespace Balea.Store.Configuration;

public class Application
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public HashSet<Role> Roles { get; set; } = [];
    public HashSet<Delegation> Delegations { get; set; } = [];
    public HashSet<Policy> Policies { get; set; } = [];
    public HashSet<Permission> Permissions { get; set; } = [];

    public HashSet<(Permission Permission, Role Role)> PermissionBindings { get; set; } = [];
    public HashSet<(string Subject, Role Role)> SubejctBindings { get; set; } = [];
}
