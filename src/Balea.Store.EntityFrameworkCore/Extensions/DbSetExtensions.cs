using Balea.Store.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore;

internal static class DbSetExtensions
{
    public static async Task<PermissionEntity> FindByNameAsync(this DbSet<PermissionEntity> dbSet, string name, CancellationToken cancellationToken = default)
        => await dbSet.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);

    public static async Task<PolicyEntity> FindByNameAsync(this DbSet<PolicyEntity> dbSet, string name, CancellationToken cancellationToken = default)
        => await dbSet.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);

    public static async Task<RoleEntity> FindByNameAsync(this DbSet<RoleEntity> dbSet, string name, CancellationToken cancellationToken = default)
        => await dbSet.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
}
