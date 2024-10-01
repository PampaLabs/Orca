using Balea.Store.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void HasApplicationFilter(this ModelBuilder modelBuilder, IAppContextAccessor appContextAccessor)
    {
        if (appContextAccessor is not null)
        {
            modelBuilder.Entity<ApplicationEntity>().HasQueryFilter(x => x.Name == appContextAccessor.AppContext.Name);
        }
    }
}
