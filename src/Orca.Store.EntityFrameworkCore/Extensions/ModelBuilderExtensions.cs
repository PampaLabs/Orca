using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

/// <summary>
/// Extension methods for <see cref="ModelBuilder" />
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures the model to use stores.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static ModelBuilder UseOrcaStores(this ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
