namespace Balea.Store.Configuration;

public class PolicyStore : IPolicyStore
{
    private readonly MemoryStoreOptions _options;
	private readonly IAppContextAccessor _contextAccessor;

	public PolicyStore(
        MemoryStoreOptions options,
		IAppContextAccessor contextAccessor)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
		_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
	}

    public Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var policy = application.Policies.FirstOrDefault(x => x.Id == policyId);

        return Task.FromResult(policy);
    }

    public Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
	{
		var application = GetCurrentApplication();

		var policy = application.Policies.FirstOrDefault(x => x.Name == policyName);

        return Task.FromResult(policy);
    }

    public Task<AccessControlResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        policy.Id = Guid.NewGuid().ToString();
        application.Policies.Add(policy);

        return Task.FromResult(AccessControlResult.Success);
	}

	public Task<AccessControlResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
	{
        var application = GetCurrentApplication();

        policy.Id = Guid.NewGuid().ToString();
        application.Policies.Remove(policy);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var application = GetCurrentApplication();

        var result = application.Policies.ToList();

        return Task.FromResult<IList<Policy>>(result);
    }

    public Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var application = GetCurrentApplication();

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

        var result = source.ToList();

        return Task.FromResult<IList<Policy>>(result);
    }

    private Application GetCurrentApplication()
    {
        return _options.Applications.GetByName(_contextAccessor.AppContext.Name);
    }
}
