using Orca.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data;

public class ContosoDbContext : OrcaDbContext
{
    public ContosoDbContext(DbContextOptions options) : base(options)
    {
    }
}
