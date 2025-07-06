using Microsoft.Extensions.Caching.Hybrid;

namespace Orca;

/// <inheritdoc />
public class CachePolicyProvider : IPolicyProvider
{
    private readonly CacheOptions _cacheOptions;

    private readonly HybridCache _cache;

    private readonly IPolicyProvider _target;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachePolicyProvider"/> class.
    /// </summary>
    /// <param name="cacheOptions">The options to configure the cache.</param>
    /// <param name="cache">The hybrid cache.</param>
    /// <param name="target">The policy resolver.</param>
    public CachePolicyProvider(
        CacheOptions cacheOptions,
        HybridCache cache,
        IPolicyProvider target
        )
    {
        _cacheOptions = cacheOptions ?? throw new ArgumentNullException(nameof(cacheOptions));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _target = target ?? throw new ArgumentNullException(nameof(target));
    }

    /// <inheritdoc />
    public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(
            key: $"Orca::Policy::{name}",
            factory: async (ct) => await _target.GetPolicyAsync(name, ct),
            options: _cacheOptions.EntryOptions,
            tags: [.._cacheOptions.Tags, "Policy"],
            cancellationToken: cancellationToken
            );
    }
}