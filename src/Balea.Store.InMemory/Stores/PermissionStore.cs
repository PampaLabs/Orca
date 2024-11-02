namespace Balea.Store.Configuration;

public class PermissionStore : IPermissionStore
{
    private readonly MemoryStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public PermissionStore(
        MemoryStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var permission = application.Permissions.FirstOrDefault(x => x.Id == permissionId);

        return Task.FromResult(permission);
    }

    public Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
	{
		var application = GetCurrentApplication();

        var permission = application.Permissions.FirstOrDefault(x => x.Name == permissionName);

        return Task.FromResult(permission);
    }

    public Task<AccessControlResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        permission.Id = Guid.NewGuid().ToString();
        application.Permissions.Add(permission);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> DeleteAsync(Permission permission, CancellationToken cancellationToken)
	{
		var application = GetCurrentApplication();

        application.Permissions.Remove(permission);
        application.PermissionBindings.RemoveWhere(binding => binding.Permission == permission);

		return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

        var result = application.PermissionBindings
            .Where(binding => binding.Permission == permission)
            .Select(binding => binding.Role)
            .ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    public Task<AccessControlResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var binding = (permission, role);
        application.PermissionBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var binding = (permission, role);
        application.PermissionBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

        var result = application.Permissions.ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    public Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

        var source = application.Permissions.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(permission => words.All(word => permission.Name.Contains(word)));
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            var words = filter.Description.Split().Where(word => word != string.Empty);
            source = source.Where(permission => words.All(word => permission.Description.Contains(word)));
        }

        if (filter.Roles is not null)
        {
            var bindings = application.PermissionBindings
                .Where(binding => filter.Roles.Contains(binding.Role.Name))
                .Select(binding => binding.Permission)
                .ToHashSet();

            source = source.Where(permission => bindings.Contains(permission));
        }

        var result = source.ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    private Application GetCurrentApplication()
    {
        return _options.Applications.GetByName(_contextAccessor.AppContext.Name);
    }
}
