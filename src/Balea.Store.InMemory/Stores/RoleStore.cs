namespace Balea.Store.Configuration;

public class RoleStore : IRoleStore
{
    private readonly MemoryStoreOptions _options;

	public RoleStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = _options.Roles.FirstOrDefault(x => x.Id == roleId);

        return Task.FromResult(role);
    }

    public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
	{
        var role = _options.Roles.FirstOrDefault(x => x.Name == roleName);

        return Task.FromResult(role);
    }

	public Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
	{
        role.Id = Guid.NewGuid().ToString();
        _options.Roles.Add(role);

        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken)
	{
        role.Id = Guid.NewGuid().ToString();
        _options.Roles.Remove(role);

        return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Roles.ToList();

        return Task.FromResult<IList<Role>>(result);
    }

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

    public Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var result = _options.SubejctBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Subject)
            .ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    public Task<AccessControlResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
	{
        var binding = (subject, role);
        _options.SubejctBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
	{
        var binding = (subject, role);
        _options.SubejctBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var result = _options.PermissionBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Permission)
            .ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    public Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = (permission, role);
        _options.PermissionBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }
}
