namespace Balea.Store.Configuration;

internal class ApplicationConfiguration
{
    public string Name { get; set; }
	public string Description { get; set; }
	public string ImageUrl { get; set; }

	public IList<RoleConfiguration> Roles { get; set; } = [];
	public IList<DelegationConfiguration> Delegations { get; set; } = [];
	public IList<PolicyConfiguration> Policies { get; set; } = [];
	public IList<PermissionConfiguration> Permissions { get; set; } = [];
}
