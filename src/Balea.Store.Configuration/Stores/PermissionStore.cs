namespace Balea.Store.Configuration;

public class PermissionStore : IPermissionStore
{
    private readonly PermissionMapper _mapper = new();

    private readonly ConfigurationStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public PermissionStore(
		ConfigurationStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permissionId);

        var permission = _mapper.FromEntity(model);

        return Task.FromResult(permission);
    }

    public Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
	{
		var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
		var model = application.Permissions.FirstOrDefault(x => x.Name == permissionName);

        var permission = _mapper.FromEntity(model);

        return Task.FromResult(permission);
    }

    public Task<AccessControlResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
	{
        var model = _mapper.ToEntity(permission);
        model.Id = Guid.NewGuid().ToString();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        application.Permissions.Add(model);

        _mapper.FromEntity(model, permission);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        _mapper.ToEntity(permission, model);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> DeleteAsync(Permission permission, CancellationToken cancellationToken)
	{
		var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        application.Permissions.Remove(model);

		return Task.FromResult(AccessControlResult.Success);
	}

    public Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleMapper();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);
        var roles = application.Roles.Where(x => model.Roles.Contains(x.Name));

        var result = roles.Select(roleMapper.FromEntity).ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    public Task<AccessControlResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        model.Roles.Add(role.Name);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Permissions.FirstOrDefault(x => x.Id == permission.Id);

        model.Roles.Remove(role.Name);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var models = application.Permissions;

        var permissions = models.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Permission>>(permissions);
    }

    public Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
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
            source = source.Where(permission => filter.Roles.Any(role => permission.Roles.Contains(role)));
        }

        var permissions = source.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Permission>>(permissions);
    }
}
