using Balea;
using Balea.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBaleaBuilder AddEntityFrameworkCoreStore(this IBaleaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        return builder;
    }

    public static IBaleaBuilder AddEntityFrameworkCoreStore(this IBaleaBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddDbContext<BaleaDbContext>(optionsAction);

        return builder;
    }

    public static IBaleaBuilder AddEntityFrameworkCoreStore(this IBaleaBuilder builder, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddDbContext<BaleaDbContext>(optionsAction);

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
