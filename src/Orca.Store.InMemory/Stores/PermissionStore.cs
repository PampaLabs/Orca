namespace Orca.Store.Configuration;

/// <inheritdoc />
public class PermissionStore : IPermissionStore
{
    private readonly MemoryStoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionStore"/> class.
    /// </summary>
    /// <param name="options">The options to configure the in-memory stores.</param>
    public PermissionStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    /// <inheritdoc />
    public Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var permission = _options.Permissions.FirstOrDefault(x => x.Id == permissionId);

        return Task.FromResult(permission);
    }

    /// <inheritdoc />
    public Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
	{
        var permission = _options.Permissions.FirstOrDefault(x => x.Name == permissionName);

        return Task.FromResult(permission);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
	{
        permission.Id = Guid.NewGuid().ToString();
        _options.Permissions.Add(permission);

        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<AccessManagementResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<AccessManagementResult> DeleteAsync(Permission permission, CancellationToken cancellationToken)
	{
        _options.Permissions.Remove(permission);
        _options.PermissionBindings.RemoveWhere(binding => binding.Permission == permission);

		return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var result = _options.PermissionBindings
            .Where(binding => binding.Permission == permission)
            .Select(binding => binding.Role)
            .ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Add(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Remove(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var result = _options.Permissions.ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _options.Permissions.AsQueryable();

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
            var bindings = _options.PermissionBindings
                .Where(binding => filter.Roles.Contains(binding.Role.Name))
                .Select(binding => binding.Permission)
                .ToHashSet();

            source = source.Where(permission => bindings.Contains(permission));
        }

        var result = source.ToList();

        return Task.FromResult<IList<Permission>>(result);
    }
}
