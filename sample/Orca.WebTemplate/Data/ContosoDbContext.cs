using Orca.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Orca.WebTemplate.Data;

public class ContosoDbContext : OrcaDbContext
{
    public ContosoDbContext(DbContextOptions options) : base(options)
    {
    }
}
