using Microsoft.EntityFrameworkCore;

using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class PermissionStore : IPermissionStore
{
    private readonly PermissionMapper _mapper = new();

    private readonly DbContext _context;

    private DbSet<PermissionEntity> Permissions => _context.Set<PermissionEntity>();

    private DbSet<RolePermissionEntity> RolePermissions => _context.Set<RolePermissionEntity>();


    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionStore"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PermissionStore(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionStore"/> class.
    /// </summary>
    /// <param name="contextAccessor">The database context accessor.</param>
    public PermissionStore(IOrcaDbContextAccessor contextAccessor)
        : this(contextAccessor.DbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var entity = await Permissions.FindAsync(permissionId, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
    {
        var entity = await Permissions.FindByNameAsync(permissionName, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(permission);
        entity.Id = Guid.NewGuid().ToString();

        await _context.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, permission);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var entity = await Permissions.FindAsync(permission.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _mapper.ToEntity(permission, entity);

        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, permission);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> DeleteAsync(Permission permission, CancellationToken cancellationToken)
    {
        var entity = await Permissions.FindAsync(permission.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleMapper();

        var targets = RolePermissions.Where(x => x.PermissionId == permission.Id);
        var roles = targets.Select(x => roleMapper.FromEntity(x.Role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var binding = new RolePermissionEntity
        {
            RoleId = role.Id,
            PermissionId = permission.Id,
        };

        await _context.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var binding = await RolePermissions
            .Where(x => x.Permission.Id == permission.Id)
            .Where(x => x.Role.Id == role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (binding is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _context.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var entities = Permissions;

        var result = entities.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Permission>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default)
    {
        var source = Permissions.AsQueryable();

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
            var bindings = RolePermissions.Where(x => filter.Roles.Contains(x.Role.Name));

            source = source.Join(
                bindings,
                permission => permission.Id,
                binding => binding.PermissionId,
                (permission, binding) => permission
            );
        }

        var permissions = source.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Permission>>(permissions);
    }
}
