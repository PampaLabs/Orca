using Orca.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data;

public class ContosoDbContext : DbContext
{
    public ContosoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOrcaStores();
    }
}
