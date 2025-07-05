namespace Orca.Store.Configuration;

/// <inheritdoc />
public class DelegationStore : IDelegationStore
{
    private readonly MemoryStoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegationStore"/> class.
    /// </summary>
    /// <param name="options">The options to configure the in-memory stores.</param>
    public DelegationStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    /// <inheritdoc />
    public Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var delegation = _options.Delegations.FirstOrDefault(x => x.Id == delegationId);

        return Task.FromResult(delegation);
    }

    /// <inheritdoc />
    public Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var delegation = _options.Delegations.FirstOrDefault(d => d.Active && d.Whom.Sub == subject);

        return Task.FromResult(delegation);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        delegation.Id = Guid.NewGuid().ToString();
        _options.Delegations.Add(delegation);

        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<AccessManagementResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        _options.Delegations.Remove(delegation);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Delegations.ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }

    /// <inheritdoc />
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
