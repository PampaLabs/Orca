using Balea;

using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Hosting
{
    public static class IHostExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this IHost host, Action<TContext, IAccessControlContext> seed = null) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var acc = scope.ServiceProvider.GetRequiredService<IAccessControlContext>();

            await context.Database.MigrateAsync();
            seed?.Invoke(context, acc);
        }
    }
}
