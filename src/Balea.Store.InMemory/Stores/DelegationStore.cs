namespace Balea.Store.Configuration;

public class DelegationStore : IDelegationStore
{
    private readonly MemoryStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public DelegationStore(
        MemoryStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var delegation = application.Delegations.FirstOrDefault(x => x.Id == delegationId);

        return Task.FromResult(delegation);
    }

    public Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var delegation = application.Delegations.GetCurrentDelegation(subject);

        return Task.FromResult(delegation);
    }

    public Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        delegation.Id = Guid.NewGuid().ToString();
        application.Delegations.Add(delegation);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        application.Delegations.Remove(delegation);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var result = application.Delegations.ToList();

        return Task.FromResult<IList<Delegation>>(result);
    }

    public Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var source = application.Delegations.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Who))
        {
            source = source.Where(delegation => delegation.Who == filter.Who);
        }

        if (!string.IsNullOrEmpty(filter.Whom))
        {
            source = source.Where(delegation => delegation.Who == filter.Whom);
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

    private Application GetCurrentApplication()
    {
        return _options.Applications.GetByName(_contextAccessor.AppContext.Name);
    }
}
