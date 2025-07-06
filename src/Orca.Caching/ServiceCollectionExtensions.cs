using Orca;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding In-memory stores to an <see cref="IOrcaBuilder"/> instance.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds hybrid cache to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddAuthorizationCache(this IOrcaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        return AddAuthorizationCache(builder, (sp, options) => { });
    }

    /// <summary>
    /// Adds hybrid cache to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="optionsAction">An action to configure <see cref="CacheOptions"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddAuthorizationCache(this IOrcaBuilder builder, Action<CacheOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddSingleton(_ => optionsAction.Build());

        return builder;
    }

    /// <summary>
    /// Adds hybrid cache to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to which the services will be added.</param>
    /// <param name="optionsAction">An action to configure <see cref="CacheOptions"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance.</returns>
    public static IOrcaBuilder AddAuthorizationCache(this IOrcaBuilder builder, Action<IServiceProvider, CacheOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddSingleton(sp => optionsAction.Build(sp));

        return builder;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        ConfigureCacheProvider<IAuthorizationContextProvider>(
            services,
            (factory, target) => factory.CreateAuthContextProvider(target));

        ConfigureCacheProvider<IPolicyProvider>(
            services,
            (factory, target) => factory.CreatePolicyProvider(target));
    }

    private static void ConfigureCacheProvider<TService>(
        IServiceCollection services,
        Func<CacheProviderFactory, TService, TService> factoryMethod
        )
        where TService : class
    {
        var descriptor = services.Last(s => s.ServiceType == typeof(TService));

        services.AddScoped(sp =>
        {
            var factory = ActivatorUtilities.CreateInstance<CacheProviderFactory>(sp);
            var target = (TService)descriptor.CreateInstance(sp);

            return factoryMethod(factory, target);
        });
    }
}
