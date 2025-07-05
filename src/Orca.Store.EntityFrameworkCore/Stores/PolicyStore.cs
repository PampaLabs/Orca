using Microsoft.EntityFrameworkCore;

using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class PolicyStore : IPolicyStore
{
    private readonly PolicyMapper _mapper = new();

    private readonly DbContext _context;

    private DbSet<PolicyEntity> Policies => _context.Set<PolicyEntity>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyStore"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PolicyStore(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyStore"/> class.
    /// </summary>
    /// <param name="contextAccessor">The database context accessor.</param>
    public PolicyStore(IOrcaDbContextAccessor contextAccessor)
        : this(contextAccessor.DbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var entity = await Policies.FindAsync(policyId, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
    {
        var entity = await Policies.FindByNameAsync(policyName, cancellationToken);
        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(policy);
        entity.Id = Guid.NewGuid().ToString();

        await _context.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, policy);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = await Policies.FindAsync(policy.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _mapper.ToEntity(policy, entity);

        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, policy);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
    {
        var entity = await Policies.FindAsync(policy.Id, cancellationToken);

        if (entity is null)
        {
            return AccessManagementResult.Failed(new AccessManagementError { Description = "Not found." });
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessManagementResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = Policies;

        var result = entities.Select(policy => _mapper.FromEntity(policy)).ToList();

        return Task.FromResult<IList<Policy>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var source = Policies.AsQueryable();

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