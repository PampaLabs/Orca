namespace Orca.Store.Configuration;

/// <inheritdoc />
public class PolicyStore : IPolicyStore
{
    private readonly MemoryStoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyStore"/> class.
    /// </summary>
    /// <param name="options">The options to configure the in-memory stores.</param>
    public PolicyStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    /// <inheritdoc />
    public Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var policy = _options.Policies.FirstOrDefault(x => x.Id == policyId);

        return Task.FromResult(policy);
    }

    /// <inheritdoc />
    public Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
	{
		var policy = _options.Policies.FirstOrDefault(x => x.Name == policyName);

        return Task.FromResult(policy);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
	{
        policy.Id = Guid.NewGuid().ToString();
        _options.Policies.Add(policy);

        return Task.FromResult(AccessManagementResult.Success);
	}

    /// <inheritdoc />
    public Task<AccessManagementResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<AccessManagementResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
	{
        policy.Id = Guid.NewGuid().ToString();
        _options.Policies.Remove(policy);

        return Task.FromResult(AccessManagementResult.Success);
    }

    /// <inheritdoc />
    public Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Policies.ToList();

        return Task.FromResult<IList<Policy>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _options.Policies.AsQueryable();

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
}
