using Balea;
using Balea.Store.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBaleaBuilder AddInMemoryStore(this IBaleaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        return AddInMemoryStore(builder, (sp, options) => { });
    }

    public static IBaleaBuilder AddInMemoryStore(this IBaleaBuilder builder, Action<MemoryStoreOptions> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddSingleton(_ => optionsAction.Build());

        return builder;
    }

    public static IBaleaBuilder AddInMemoryStore(this IBaleaBuilder builder, Action<IServiceProvider, MemoryStoreOptions> optionsAction)
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
    }
}
