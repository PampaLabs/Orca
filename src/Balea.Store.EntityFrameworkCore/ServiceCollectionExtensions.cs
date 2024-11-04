using Balea;
using Balea.Store.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBaleaBuilder AddEntityFrameworkStores(this IBaleaBuilder builder)
        => AddEntityFrameworkStores<BaleaDbContext>(builder);

    public static IBaleaBuilder AddEntityFrameworkStores<TDbContext>(this IBaleaBuilder builder)
        where TDbContext : BaleaDbContext
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        ConfigureServices(builder.Services);

        builder.Services.AddScoped<BaleaDbContext, TDbContext>();

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
