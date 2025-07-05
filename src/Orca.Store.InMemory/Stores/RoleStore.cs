namespace Orca.Store.Configuration;

/// <inheritdoc />
public class RoleStore : IRoleStore
{
    private readonly MemoryStoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="options">The options to configure the in-memory stores.</param>
    public RoleStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    /// <inheritdoc />
    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = _options.Roles.FirstOrDefault(x => x.Id == roleId);

        return Task.FromResult(role);
    }

    /// <inheritdoc />
    public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
	{
        var role = _options.Roles.FirstOrDefault(x => x.Name == roleName);

        return Task.FromResult(role);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> CreateAsync(Role role, CancellationToken cancellationToken)
	{
        role.Id = Guid.NewGuid().ToString();
        _options.Roles.Add(role);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> UpdateAsync(Role role, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<AccessManagementResult> DeleteAsync(Role role, CancellationToken cancellationToken)
	{
        role.Id = Guid.NewGuid().ToString();
        _options.Roles.Remove(role);

        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Roles.ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _options.Roles.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(role => words.All(word => role.Name.Contains(word)));
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            var words = filter.Description.Split().Where(word => word != string.Empty);
            source = source.Where(role => words.All(word => role.Description.Contains(word)));
        }

        if (filter.Enabled.HasValue)
        {
            source = source.Where(role => role.Enabled == filter.Enabled.Value);
        }

        if (filter.Mappings is not null)
        {
            source = source.Where(role => filter.Mappings.Any(mapping => role.Mappings.Contains(mapping)));
        }

        var result = source.ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var result = _options.SubejctBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Subject)
            .ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
	{
        var binding = (subject, role);
        _options.SubejctBindings.Add(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
	{
        var binding = (subject, role);
        _options.SubejctBindings.Remove(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var result = _options.PermissionBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Permission)
            .ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Add(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Remove(binding);

        return Task.FromResult(AccessManagementResult.Success);
    }
}
