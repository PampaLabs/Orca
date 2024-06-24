using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

public class PolicyStore : IPolicyStore
{
    private readonly PolicyMapper _mapper = new();

    private readonly BaleaDbContext _context;

    public PolicyStore(BaleaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var entity = await _context.Policies.FindAsync(policyId, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    public async Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
    {
        var entity = await _context.Policies.FindByNameAsync(policyName, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    public async Task<AccessControlResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(policy);
        entity.Id = Guid.NewGuid().ToString();

        await _context.Policies.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, policy);

        return AccessControlResult.Success;
    }

    public async Task<AccessControlResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = await _context.Policies.FindAsync(policy.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _mapper.ToEntity(policy, entity);

        _context.Policies.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, policy);

        return AccessControlResult.Success;
    }

    public async Task<AccessControlResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = await _context.Policies.FindAsync(policy.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.Policies.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    public Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = _context.Policies;

        var result = entities.Select(policy => _mapper.FromEntity(policy)).ToList();

        return Task.FromResult<IList<Policy>>(result);
    }

    public Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _context.Policies.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(policy => words.All(word => policy.Name.Contains(word)));
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            var words = filter.Description.Split().Where(word => word != string.Empty);
            source = source.Where(policy => words.All(word => policy.Description.Contains(word)));
        }

        var result = source.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Policy>>(result);
    }
}