using Balea;
using Balea.Store.Configuration;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBaleaBuilder AddConfigurationStore(this IBaleaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        return AddConfigurationStore(builder, (sp, options) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            configuration.GetSection("Balea").Bind(options);
        });
    }

    public static IBaleaBuilder AddConfigurationStore(this IBaleaBuilder builder, Action<ConfigurationStoreOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddScoped(_ => optionsAction.Build());

        return builder;
    }

    public static IBaleaBuilder AddConfigurationStore(this IBaleaBuilder builder, Action<IServiceProvider, ConfigurationStoreOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddScoped(sp => optionsAction.Build(sp));

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
