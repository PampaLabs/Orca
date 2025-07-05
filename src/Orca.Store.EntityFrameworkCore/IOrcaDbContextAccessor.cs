using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

public interface IOrcaDbContextAccessor
{
    DbContext DbContext { get; }
}

internal class OrcaDbContextAccessor : IOrcaDbContextAccessor
{
    public DbContext DbContext { get; set; }
}
