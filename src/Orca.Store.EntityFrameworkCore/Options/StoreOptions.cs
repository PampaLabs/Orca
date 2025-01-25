using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore.Options;

/// <summary>
/// Provide programatically configuration for <see cref="OrcaDbContext"/>.
/// </summary>
public class StoreOptions
{
    public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
}