using Orca;
using Orca.Store.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding EntityFramework stores to an <see cref="IOrcaBuilder"/> instance.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds EntityFramework stores to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddEntityFrameworkStores(this IOrcaBuilder builder)
        => AddEntityFrameworkStores<OrcaDbContext>(builder);

    /// <summary>
    /// Adds EntityFramework stores to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddEntityFrameworkStores<TDbContext>(this IOrcaBuilder builder)
        where TDbContext : OrcaDbContext
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddScoped<OrcaDbContext, TDbContext>();

        return builder;
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IDelegationStore, DelegationStore>();
        services.AddScoped<IPermissionStore, PermissionStore>();
        services.AddScoped<IPolicyStore, PolicyStore>();
        services.AddScoped<IRoleStore, RoleStore>();
        services.AddScoped<ISubjectStore, SubjectStore>();
    }
}
