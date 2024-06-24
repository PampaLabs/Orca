using Balea;
using Balea.Store.EntityFrameworkCore;
using Balea.Store.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public static class IHostExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this IWebHost host)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TContext>();

                if (context is null) return;

                try
                {
                    // await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

                    logger.LogError(ex, $"An error occurred while migrating the database for context {nameof(TContext)}.");
                }
            }
        }

        public static async Task SeedDbContextAsync<TContext>(this IWebHost host)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<BaleaDbContext>();

                if (context is null) return;

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
}
