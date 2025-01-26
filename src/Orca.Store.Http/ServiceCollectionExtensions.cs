using Orca;
using Orca.Store.Http;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding HTTP-based stores to an <see cref="IOrcaBuilder"/> instance.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds HTTP-based stores to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddHttpStores(this IOrcaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        return builder;
    }

    /// <summary>
    /// Adds HTTP-based stores to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="configureClient">An action to configure <see cref="HttpClient"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddHttpStores(this IOrcaBuilder builder, Action<HttpClient> configureClient)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddHttpClient(HttpStoreDefaults.HttpClientName)
            .ConfigureHttpClient(configureClient);

        return builder;
    }

    /// <summary>
    /// Adds the default HTTP-based stores to the <see cref="IOrcaBuilder"/> with custom configuration options.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="configureClient">An action to configure <see cref="HttpClient"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddHttpStores(this IOrcaBuilder builder, Action<IServiceProvider, HttpClient> configureClient)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddHttpClient(HttpStoreDefaults.HttpClientName)
            .ConfigureHttpClient(configureClient);

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
