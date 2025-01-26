using Orca.Store.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

/// <inheritdoc />
public class DelegationStore : IDelegationStore
{
    private readonly DelegationMapper _mapper = new();

    private readonly OrcaDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegationStore"/> class.
    /// </summary>
    /// <param name="context">The database context used to interact with the data store.</param>
    public DelegationStore(OrcaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var entity = await _context.Delegations
            .Include(x => x.Who)
            .Include(x => x.Whom)
            .Where(x => x.Id == delegationId)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var entity = await _context.Delegations
            .Include(x => x.Who)
            .Include(x => x.Whom)
            .Where(x => x.Enabled)
            .Where(x => x.From <= now && x.To >= now)
            .Where(x => x.Whom.Sub == subject)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.FromEntity(entity);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(delegation);
        entity.Id = Guid.NewGuid().ToString();

        await _context.Delegations.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, delegation);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var entity = await _context.Delegations.FindAsync(delegation.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _mapper.ToEntity(delegation, entity);

        _context.Delegations.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _mapper.FromEntity(entity, delegation);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var entity = await _context.Delegations.FindAsync(delegation.Id, cancellationToken);

        if (entity is null)
        {
            return AccessControlResult.Failed(new AccessControlError { Description = "Not found." });
        }

        _context.Delegations.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessControlResult.Success;
    }

    /// <inheritdoc />
    public Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = _context.Delegations
            .Include(x => x.Who)
            .Include(x => x.Whom);

        var result = entities.Select(delegation => _mapper.FromEntity(delegation)).ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken)
    {
        var source = _context.Delegations
            .Include(x => x.Who)
            .Include(x => x.Whom)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Who))
        {
            source = source.Where(delegation => delegation.Who.Sub == filter.Who);
        }

        if (!string.IsNullOrEmpty(filter.Whom))
        {
            source = source.Where(delegation => delegation.Who.Sub == filter.Whom);
        }

        if (filter.From.HasValue)
        {
            source = source.Where(delegation => delegation.From >= filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            source = source.Where(delegation => delegation.To <= filter.To.Value);
        }

        if (filter.Enabled.HasValue)
        {
            source = source.Where(delegation => delegation.Enabled == filter.Enabled.Value);
        }

        var result = source.Select(delegation => _mapper.FromEntity(delegation)).ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }
}
