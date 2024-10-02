namespace Balea.Store.Configuration;

public class RoleStore : IRoleStore
{
    private readonly RoleMapper _mapper = new();

    private readonly ConfigurationStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public RoleStore(
		ConfigurationStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == roleId);

        var role = _mapper.FromEntity(model);

        return Task.FromResult(role);
    }

    public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
	{
		var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
		var model = application.Roles.FirstOrDefault(x => x.Name == roleName);

        var role = _mapper.FromEntity(model);

        return Task.FromResult(role);
    }

	public Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
	{
        var model = _mapper.ToEntity(role);
        model.Id = Guid.NewGuid().ToString();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        application.Roles.Add(model);

        _mapper.FromEntity(model, role);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);

        _mapper.ToEntity(role, model);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);

        application.Roles.Remove(model);

        return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var models = application.Roles;

        var roles = models.Select(role => _mapper.FromEntity(role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
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
            source = source.Where(role => filter.Subjects.Any(subject => role.Subjects.Contains(subject)));
        }

        var roles = source.Select(role => _mapper.FromEntity(role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
    }

    public Task<IList<string>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);

        var mappings = model.Subjects.ToList();

        return Task.FromResult<IList<string>>(mappings);
    }

    public Task<AccessControlResult> AddSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);

        model.Subjects.Add(subject);

		return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> RemoveSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);

        model.Subjects.Remove(subject);

		return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var permissionMapper = new PermissionMapper();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Roles.FirstOrDefault(x => x.Id == role.Id);
        var permissions = application.Permissions.Where(x => x.Roles.Contains(model.Name));

        var result = permissions.Select(permissionMapper.FromEntity).ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    public Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        model.Roles.Add(role.Name);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        model.Roles.Remove(role.Name);

        return Task.FromResult(AccessControlResult.Success);
    }
}
