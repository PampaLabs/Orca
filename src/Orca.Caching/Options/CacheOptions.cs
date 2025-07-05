using Microsoft.Extensions.Caching.Hybrid;

namespace Orca;

/// <summary>
/// Options for configuring in-memory stores.
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// The tags to associate with the cache items.
    /// </summary>
    public IEnumerable<string> Tags { get; set; } = ["Orca"];

    /// <summary>
    /// Additional options for the cache entries.
    /// </summary>
    public HybridCacheEntryOptions EntryOptions { get; set; }
}
