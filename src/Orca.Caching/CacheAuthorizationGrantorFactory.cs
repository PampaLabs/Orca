using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace Orca;

internal class CacheAuthorizationGrantorFactory(
    IOptions<OrcaOptions> options,
    CacheOptions cacheOptions,
    HybridCache cache
    )
{
    public CacheAuthorizationGrantor Create(IAuthorizationGrantor target)
    {
        return new(options, cacheOptions, cache, target);
    }
}