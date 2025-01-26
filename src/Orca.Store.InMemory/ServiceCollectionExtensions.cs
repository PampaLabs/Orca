using Orca;
using Orca.Store.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding In-memory stores to an <see cref="IOrcaBuilder"/> instance.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds In-memory stores to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddInMemoryStores(this IOrcaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        return AddInMemoryStores(builder, (sp, options) => { });
    }

    /// <summary>
    /// Adds In-memory stores to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="optionsAction">An action to configure <see cref="MemoryStoreOptions"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddInMemoryStores(this IOrcaBuilder builder, Action<MemoryStoreOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddSingleton(_ => optionsAction.Build());

        return builder;
    }

    /// <summary>
    /// Adds In-memory stores to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="optionsAction">An action to configure <see cref="MemoryStoreOptions"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddInMemoryStores(this IOrcaBuilder builder, Action<IServiceProvider, MemoryStoreOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddSingleton(sp => optionsAction.Build(sp));

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
