using Balea.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Balea.WebTemplate.Data;

public class ContosoDbContext : BaleaDbContext
{
    public ContosoDbContext(DbContextOptions options) : base(options)
    {
    }
}
