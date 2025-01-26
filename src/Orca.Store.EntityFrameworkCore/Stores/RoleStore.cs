using Microsoft.EntityFrameworkCore;

using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class RoleStore : IRoleStore
{
    private readonly RoleMapper _mapper = new();

    private readonly OrcaDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="context">The database context used to interact with the data store.</param>
    public RoleStore(OrcaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Id == roleId, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Name == roleName, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(role);
        entity.Id = Guid.NewGuid().ToString();

        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, role);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .Include(role => role.Mappings)
            .FirstOrDefaultAsync(role => role.Id == role.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _mapper.ToEntity(role, entity);

        _context.Roles.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, role);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles.FindAsync(role.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.Roles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = _context.Roles;

        var result = entities
            .Include(role => role.Mappings)
            .Select(role => _mapper.FromEntity(role)).ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _context.Roles.AsQueryable();

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
            var bindings = _context.RoleMappings.Where(x => filter.Mappings.Contains(x.Mapping));

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

        var targets = _context.RoleSubjects.Where(x => x.RoleId == role.Id);
        var subjects = targets.Select(x => subjectMapper.FromEntity(x.Subject)).ToList();

        return Task.FromResult<IList<Subject>>(subjects);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles.FindAsync(role.Id, cancellationToken);

        var binding = new RoleSubjectEntity
        {
            RoleId = entity.Id,
            SubjectId = subject.Id
        };

        await _context.RoleSubjects.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var binding = await _context.RoleSubjects
            .Where(x => x.RoleId == role.Id)
            .Where(x => x.SubjectId == subject.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (binding is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.RoleSubjects.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var permissionMapper = new PermissionMapper();

        var targets = _context.RolePermissions.Where(x => x.RoleId == role.Id);
        var permissions = targets.Select(x => permissionMapper.FromEntity(x.Permission)).ToList();

        return Task.FromResult<IList<Permission>>(permissions);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = new RolePermissionEntity
        {
            RoleId = role.Id,
            PermissionId = permission.Id,
        };

        await _context.RolePermissions.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var binding = await _context.RolePermissions
            .Where(x => x.Permission.Id == permission.Id)
            .Where(x => x.Role.Id == role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (binding is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.RolePermissions.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }
}
