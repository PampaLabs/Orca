using Orca;
using Orca.Store.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IOrcaBuilder AddEntityFrameworkStores(this IOrcaBuilder builder)
        => AddEntityFrameworkStores<OrcaDbContext>(builder);

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
