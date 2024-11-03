using Microsoft.EntityFrameworkCore;

using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

public class SubjectStore : ISubjectStore
{
    private readonly SubjectMapper _mapper = new();

    private readonly BaleaDbContext _context;

    public SubjectStore(BaleaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(subjct => subjct.Id == subjectId, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    public async Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(user => user.Sub == sub, cancellationToken);

        return _mapper.FromEntity(entity);
    }

    public async Task<AccessControlResult> CreateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(subject);
        entity.Id = Guid.NewGuid().ToString();

        await _context.Subjects.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, subject);

        return AccessControlResult.Success;
    }

    public async Task<AccessControlResult> UpdateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(user => user.Id == user.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _mapper.ToEntity(subject, entity);

        _context.Subjects.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, subject);

        return AccessControlResult.Success;
    }

    public async Task<AccessControlResult> DeleteAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects.FindAsync(subject.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.Subjects.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    public Task<IList<Subject>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = _context.Subjects;

        var result = entities
            .Select(user => _mapper.FromEntity(user)).ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    public Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _context.Subjects.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(user => words.All(word => user.Name.Contains(word)));
        }

        var users = source
            .Select(subject => _mapper.FromEntity(subject)).ToList();

        return Task.FromResult<IList<Subject>>(users);
    }

    public Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleMapper();

        var targets = _context.RoleSubjects.Where(x => x.SubjectId == subject.Id);
        var roles = targets.Select(x => roleMapper.FromEntity(x.Role)).ToList();

        return Task.FromResult<IList<Role>>(roles);
    }

    public async Task<AccessControlResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var binding = new RoleSubjectEntity
        {
            SubjectId = subject.Id,
            RoleId = role.Id,
        };

        await _context.RoleSubjects.AddAsync(binding, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    public async Task<AccessControlResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var binding = await _context.RoleSubjects
            .Where(x => x.SubjectId == subject.Id)
            .Where(x => x.RoleId == role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (binding is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.RoleSubjects.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }
}
