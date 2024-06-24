using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore.Options;

/// <summary>
/// Provide programatically configuration for <see cref="BaleaDbContext"/>.
/// </summary>
public class StoreOptions
{
    public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
}