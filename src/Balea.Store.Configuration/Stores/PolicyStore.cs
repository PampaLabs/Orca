namespace Balea.Store.Configuration;

public class PolicyStore : IPolicyStore
{
    private readonly PolicyMapper _mapper = new();

    private readonly ConfigurationStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public PolicyStore(
		ConfigurationStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var entity = application.Policies.FirstOrDefault(x => x.Id == policyId);

        var policy = _mapper.FromEntity(entity);

        return Task.FromResult(policy);
    }

    public Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
	{
		var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
		var entity = application.Policies.FirstOrDefault(x => x.Name == policyName);

        var policy = _mapper.FromEntity(entity);

        return Task.FromResult(policy);
    }

    public Task<AccessControlResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
	{
        var model = _mapper.ToEntity(policy);
        model.Id = Guid.NewGuid().ToString();

        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
		application.Policies.Add(model);

        _mapper.FromEntity(model, policy);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Policies.FirstOrDefault(x => x.Id == policy.Id);

        _mapper.ToEntity(policy, model);

        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
	{
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var model = application.Policies.FirstOrDefault(x => x.Id == policy.Id);

        application.Policies.Remove(model);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var models = application.Policies;

        var policies = models.Select(policy => _mapper.FromEntity(policy)).ToList();

        return Task.FromResult<IList<Policy>>(policies);
    }

    public Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var application = _options.Applications.GetByName(_contextAccessor.AppContext.Name);
        var source = application.Policies.AsQueryable();

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

        var policies = source.Select(_mapper.FromEntity).ToList();

        return Task.FromResult<IList<Policy>>(policies);
    }
}
