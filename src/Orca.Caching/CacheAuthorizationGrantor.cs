using System.Security.Claims;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace Orca;

/// <inheritdoc />
public class CacheAuthorizationGrantor : IAuthorizationGrantor
{
    private readonly OrcaOptions _options;

    private readonly CacheOptions _cacheOptions;

    private readonly HybridCache _cache;

    private readonly IAuthorizationGrantor _target;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheAuthorizationGrantor"/> class.
    /// </summary>
    /// <param name="options">The authorizations options.</param>
    /// <param name="cacheOptions">The options to configure the cache.</param>
    /// <param name="cache">The hybrid cache.</param>
    /// <param name="target">The authorization grantor.</param>
    public CacheAuthorizationGrantor(
        IOptions<OrcaOptions> options,
        CacheOptions cacheOptions,
        HybridCache cache,
        IAuthorizationGrantor target
        )
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _cacheOptions = cacheOptions ?? throw new ArgumentNullException(nameof(cacheOptions));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _target = target ?? throw new ArgumentNullException(nameof(target));
    }

    /// <inheritdoc />
    public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var subject = principal.GetSubjectId(_options);

        return await _cache.GetOrCreateAsync(
            key: $"Orca::Authorization::{subject}",
            factory: async (ct) => await _target.FindAuthorizationAsync(principal, ct),
            options: _cacheOptions.EntryOptions,
            tags: [.._cacheOptions.Tags, "Authorization"],
            cancellationToken: cancellationToken
            );
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