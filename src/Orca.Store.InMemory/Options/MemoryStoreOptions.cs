namespace Orca.Store.Configuration;

public class MemoryStoreOptions
{
    public HashSet<Subject> Subjects { get; set; } = [];
    public HashSet<Role> Roles { get; set; } = [];
    public HashSet<Delegation> Delegations { get; set; } = [];
    public HashSet<Policy> Policies { get; set; } = [];
    public HashSet<Permission> Permissions { get; set; } = [];

    public HashSet<(Permission Permission, Role Role)> PermissionBindings { get; set; } = [];
    public HashSet<(Subject Subject, Role Role)> SubejctBindings { get; set; } = [];
}
