namespace Balea.Store.Configuration;

internal class BaleaConfiguration
{
    public IList<RoleConfiguration> Roles { get; set; } = [];
    public IList<DelegationConfiguration> Delegations { get; set; } = [];
    public IList<PolicyConfiguration> Policies { get; set; } = [];
    public IList<PermissionConfiguration> Permissions { get; set; } = [];
}
