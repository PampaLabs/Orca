using Balea.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data;

public class ContosoDbContext : BaleaDbContext
{
    public ContosoDbContext(DbContextOptions options) : base(options)
    {
    }
}
