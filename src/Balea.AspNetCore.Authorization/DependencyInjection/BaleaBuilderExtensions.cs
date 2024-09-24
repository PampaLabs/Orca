using Balea;
using Balea.Authorization;
using Balea.Authorization.Abac;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class BaleaBuilderExtensions
{
    public static IBaleaBuilder AddAuthorization(this IBaleaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        AddAuthorization(builder, _ => { });

        return builder;
    }

    public static IBaleaBuilder AddAuthorization(this IBaleaBuilder builder, Action<BaleaWebHost> options)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        //add balea required services
        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        builder.Services.AddScoped<IAspNetPropertyBag, UserPropertyBag>();
        builder.Services.AddScoped<IAspNetPropertyBag, ResourcePropertyBag>();
        builder.Services.AddScoped<IAspNetPropertyBag, ParameterPropertyBag>();

        builder.Services.AddScoped<AbacAuthorizationContextFactory>();

        builder.Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddTransient<IAuthorizationHandler, AbacAuthorizationHandler>();
        builder.Services.AddTransient<IPolicyEvaluator, BaleaPolicyEvaluator>();

        builder.Services.Configure(options);

        return builder;
    }
}
