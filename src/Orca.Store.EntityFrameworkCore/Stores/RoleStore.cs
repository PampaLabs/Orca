using Microsoft.EntityFrameworkCore;

using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class RoleStore : IRoleStore
{
    private readonly RoleMapper _mapper = new();

    private readonly DbContext _context;

    private DbSet<RoleEntity> Roles => _context.Set<RoleEntity>();

    private DbSet<RoleMappingEntity> RoleMappings => _context.Set<RoleMappingEntity>();

    private DbSet<RoleSubjectEntity> RoleSubjects => _context.Set<RoleSubjectEntity>();

    private DbSet<RolePermissionEntity> RolePermissions => _context.Set<RolePermissionEntity>();


    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public RoleStore(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="contextAccessor">The database context accessor.</param>
    public RoleStore(IOrcaDbContextAccessor contextAccessor)
        : this(contextAccessor.DbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var entity = await Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Id == roleId, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        var entity = await Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Name == roleName, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(role);
        entity.Id = Guid.NewGuid().ToString();

        await _context.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, role);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = await Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Id == role.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _mapper.ToEntity(role, entity);

        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, role);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = await Roles.FindAsync(role.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = Roles;

        var result = entities
            .Include(role => role.Mappings)
            .Select(role => _mapper.FromEntity(role)).ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var source = Roles.AsQueryable();

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
            var bindings = RoleMappings.Where(x => filter.Mappings.Contains(x.Mapping));

            source = source.Join(
                bindings,
                role => role.Id,
                binding => binding.RoleId,
                (role, binding) => role
            );
        }

        var roles = source
            .Include(role => role.Mappings)
            .Select(role => _mapper.FromEntity(role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
    }

    /// <inheritdoc />
    public Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var subjectMapper = new SubjectMapper();

        var targets = RoleSubjects.Where(x => x.RoleId == role.Id);
        var subjects = targets.Select(x => subjectMapper.FromEntity(x.Subject)).ToList();

        return Task.FromResult<IList<Subject>>(subjects);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var entity = await Roles.FindAsync(role.Id, cancellationToken);

        var binding = new RoleSubjectEntity
        {
            RoleId = entity.Id,
            SubjectId = subject.Id
        };

        await _context.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var binding = await RoleSubjects
            .Where(x => x.RoleId == role.Id)
            .Where(x => x.SubjectId == subject.Id)
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
    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var permissionMapper = new PermissionMapper();

        var targets = RolePermissions.Where(x => x.RoleId == role.Id);
        var permissions = targets.Select(x => permissionMapper.FromEntity(x.Permission)).ToList();

        return Task.FromResult<IList<Permission>>(permissions);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
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
    public async Task<AccessManagementResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
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
}
