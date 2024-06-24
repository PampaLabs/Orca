namespace Balea.Store.Configuration;

public class DelegationStore : IDelegationStore
{
    private readonly DelegationMapper _mapper = new();

    private readonly ConfigurationStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public DelegationStore(
		ConfigurationStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Delegations.FirstOrDefault(x => x.Id == delegationId);

        var delegation = _mapper.FromEntity(model);

        return Task.FromResult(delegation);
    }

    public Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Delegations.GetCurrentDelegation(subject);

        var delegation = _mapper.FromEntity(model);

        return Task.FromResult(delegation);
    }

    public Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        var model = _mapper.ToEntity(delegation);
        model.Id = Guid.NewGuid().ToString();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        application.Delegations.Add(model);

        _mapper.FromEntity(model, delegation);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Delegations.FirstOrDefault(x => x.Id == delegation.Id);

        _mapper.ToEntity(delegation, model);

        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Delegations.FirstOrDefault(x => x.Id == delegation.Id);

        application.Delegations.Remove(model);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var models = application.Delegations;

        var delegations = models.Select(delegation => _mapper.FromEntity(delegation)).ToList();

        return Task.FromResult<IList<Delegation>>(delegations);
    }

    public Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
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

        var delegations = source.Select(delegation => _mapper.FromEntity(delegation)).ToList();

        return Task.FromResult<IList<Delegation>>(delegations);
    }
}
