namespace Balea.Store.Configuration;

public class RoleStore : IRoleStore
{
    private readonly MemoryStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public RoleStore(
        MemoryStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var role = application.Roles.FirstOrDefault(x => x.Id == roleId);

        return Task.FromResult(role);
    }

    public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        var role = application.Roles.FirstOrDefault(x => x.Name == roleName);

        return Task.FromResult(role);
    }

	public Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        role.Id = Guid.NewGuid().ToString();
        application.Roles.Add(role);

        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        role.Id = Guid.NewGuid().ToString();
        application.Roles.Remove(role);

        return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var result = application.Roles.ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    public Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var source = application.Roles.AsQueryable();

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

        if (filter.Subjects is not null)
        {
            var bindings = application.SubejctBindings
                .Where(binding => filter.Subjects.Contains(binding.Subject))
                .Select(binding => binding.Role)
                .ToHashSet();

            source = source.Where(role => bindings.Contains(role));
        }

        var result = source.ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    public Task<IList<string>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

        var result = application.SubejctBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Subject)
            .ToList();

        return Task.FromResult<IList<string>>(result);
    }

    public Task<AccessControlResult> AddSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        var binding = (subject, role);
        application.SubejctBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemoveSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        var binding = (subject, role);
        application.SubejctBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

        var result = application.PermissionBindings
            .Where(binding => binding.Role == role)
            .Select(binding => binding.Permission)
            .ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    public Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var binding = (permission, role);
        application.PermissionBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var binding = (permission, role);
        application.PermissionBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    private Application GetCurrentApplication()
    {
        return _options.Applications.GetByName(_contextAccessor.AppContext.Name);
    }
}
