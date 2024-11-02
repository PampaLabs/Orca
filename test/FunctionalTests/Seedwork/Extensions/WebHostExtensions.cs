using Balea;
using Balea.Store.EntityFrameworkCore;
using Balea.Store.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class WebHostExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this IWebHost host)
            where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            // await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }

        public static async Task SeedDbContextAsync<TContext>(this IWebHost host)
            where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BaleaDbContext>();

            var application = new ApplicationEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = BaleaConstants.DefaultApplicationName,
                Description = "Default Application",
            };

            await context.Applications.AddAsync(application);
            await context.SaveChangesAsync();
        }
    }
}
