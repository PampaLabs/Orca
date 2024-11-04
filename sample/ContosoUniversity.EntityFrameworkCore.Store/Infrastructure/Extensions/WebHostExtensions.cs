using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Hosting
{
    public static class WebHostExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this IApplicationBuilder host)
            where TContext : DbContext
        {
            using var scope = host.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            // await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
}
