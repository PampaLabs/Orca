namespace Orca.Store.Configuration;

public class DelegationStore : IDelegationStore
{
    private readonly MemoryStoreOptions _options;

	public DelegationStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    public Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var delegation = _options.Delegations.FirstOrDefault(x => x.Id == delegationId);

        return Task.FromResult(delegation);
    }

    public Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var delegation = _options.Delegations.FirstOrDefault(d => d.Active && d.Whom.Sub == subject);

        return Task.FromResult(delegation);
    }

    public Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        delegation.Id = Guid.NewGuid().ToString();
        _options.Delegations.Add(delegation);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        _options.Delegations.Remove(delegation);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Delegations.ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }

    public Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken)
    {
        var source = _options.Delegations.AsQueryable();

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

        var result = source.ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }
}
