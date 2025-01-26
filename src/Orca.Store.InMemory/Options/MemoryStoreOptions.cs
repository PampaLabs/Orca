namespace Orca.Store.Configuration;

/// <summary>
/// Options for configuring in-memory stores.
/// </summary>
public class MemoryStoreOptions
{
    /// <summary>
    /// Gets or sets the set of subjects in the memory store.
    /// </summary>
    public HashSet<Subject> Subjects { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of roles in the memory store.
    /// </summary>
    public HashSet<Role> Roles { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of delegations in the memory store.
    /// </summary>
    public HashSet<Delegation> Delegations { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of policies in the memory store.
    /// </summary>
    public HashSet<Policy> Policies { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of permissions in the memory store.
    /// </summary>
    public HashSet<Permission> Permissions { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of permission bindings, associating permissions with roles.
    /// </summary>
    public HashSet<(Permission Permission, Role Role)> PermissionBindings { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of subject bindings, associating subjects with roles.
    /// </summary>
    public HashSet<(Subject Subject, Role Role)> SubejctBindings { get; set; } = [];
}
