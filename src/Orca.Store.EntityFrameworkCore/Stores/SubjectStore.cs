using Microsoft.EntityFrameworkCore;

using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class SubjectStore : ISubjectStore
{
    private readonly SubjectMapper _mapper = new();

    private readonly DbContext _context;

    private DbSet<SubjectEntity> Subjects => _context.Set<SubjectEntity>();

    private DbSet<RoleSubjectEntity> RoleSubjects => _context.Set<RoleSubjectEntity>();

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectStore"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SubjectStore(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="contextAccessor">The database context accessor.</param>
    public SubjectStore(IOrcaDbContextAccessor contextAccessor)
        : this(contextAccessor.DbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken)
    {
        var entity = await Subjects
            .FirstOrDefaultAsync(subjct => subjct.Id == subjectId, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken)
    {
        var entity = await Subjects
            .FirstOrDefaultAsync(user => user.Sub == sub, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> CreateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(subject);
        entity.Id = Guid.NewGuid().ToString();

        await _context.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, subject);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> UpdateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = await Subjects
            .FirstOrDefaultAsync(user => user.Id == user.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _mapper.ToEntity(subject, entity);

        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, subject);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> DeleteAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = await Subjects.FindAsync(subject.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Subject>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = Subjects;

        var result = entities
            .Select(user => _mapper.FromEntity(user)).ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default)
    {
        var source = Subjects.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(user => words.All(word => user.Name.Contains(word)));
        }

        var users = source
            .Select(subject => _mapper.FromEntity(subject)).ToList();

        return Task.FromResult<IList<Subject>>(users);
    }

    /// <inheritdoc />
    public Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleMapper();

        var targets = RoleSubjects.Where(x => x.SubjectId == subject.Id);
        var roles = targets.Select(x => roleMapper.FromEntity(x.Role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var binding = new RoleSubjectEntity
        {
            SubjectId = subject.Id,
            RoleId = role.Id,
        };

        await _context.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var binding = await RoleSubjects
            .Where(x => x.SubjectId == subject.Id)
            .Where(x => x.RoleId == role.Id)
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
