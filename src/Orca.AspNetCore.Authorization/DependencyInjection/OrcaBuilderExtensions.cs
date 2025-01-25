using Orca;
using Orca.Authorization;
using Orca.Authorization.Abac;
using Orca.Authorization.Abac.Context;
using Orca.Authorization.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Microsoft.Extensions.DependencyInjection;

public static class OrcaBuilderExtensions
{
    public static IOrcaBuilder AddAuthorization(this IOrcaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        AddAuthorization(builder, _ => { });

        return builder;
    }

    public static IOrcaBuilder AddAuthorization(this IOrcaBuilder builder, Action<OrcaWebHost> options)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        builder.Services.AddScoped<IAspNetPropertyBag, UserPropertyBag>();
        builder.Services.AddScoped<IAspNetPropertyBag, ResourcePropertyBag>();
        builder.Services.AddScoped<IAspNetPropertyBag, ParameterPropertyBag>();

        builder.Services.AddScoped<AbacAuthorizationContextFactory>();

        builder.Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddTransient<IAuthorizationHandler, AbacAuthorizationHandler>();
        builder.Services.AddTransient<IPolicyEvaluator, OrcaPolicyEvaluator>();

        builder.Services.Configure(options);

        return builder;
    }
}
