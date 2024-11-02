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

        public static Task SeedDbContextAsync<TContext>(this IWebHost host)
            where TContext : DbContext
        {
            return Task.CompletedTask;
        }
    }
}
