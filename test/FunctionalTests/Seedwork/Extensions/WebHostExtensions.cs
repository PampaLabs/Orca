using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class WebHostExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            // await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
}
