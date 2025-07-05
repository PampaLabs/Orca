using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

/// <summary>
/// Provides access to the <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.
/// </summary>
public interface IOrcaDbContextAccessor
{
    /// <summary>
    /// Gets the <see cref="Microsoft.EntityFrameworkCore.DbContext"/>
    /// </summary>
    DbContext DbContext { get; }
}


internal class OrcaDbContextAccessor : IOrcaDbContextAccessor
{
    public DbContext DbContext { get; set; }
}
