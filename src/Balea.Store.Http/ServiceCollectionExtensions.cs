using Balea;
using Balea.Store;
using Balea.Store.Http;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBaleaBuilder AddHttpStore(this IBaleaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        return builder;
    }

    public static IBaleaBuilder AddHttpStore(this IBaleaBuilder builder, Action<HttpClient> configureClient)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddHttpClient(HttpStoreDefaults.HttpClientName)
            .ConfigureHttpClient(configureClient);

        return builder;
    }

    public static IBaleaBuilder AddHttpStore(this IBaleaBuilder builder, Action<IServiceProvider, HttpClient> configureClient)
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
    }
}
