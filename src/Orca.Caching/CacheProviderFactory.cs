using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace Orca;

internal class CacheProviderFactory(
    IOptions<OrcaOptions> options,
    CacheOptions cacheOptions,
    HybridCache cache
    )
{
    public CacheAuthorizationContextProvider CreateAuthContextProvider(IAuthorizationContextProvider target)
    {
        return new(options, cacheOptions, cache, target);
    }

    public CachePolicyProvider CreatePolicyProvider(IPolicyProvider target)
    {
        return new(cacheOptions, cache, target);
    }
}