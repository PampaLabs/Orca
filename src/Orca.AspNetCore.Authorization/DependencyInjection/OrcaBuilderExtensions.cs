using Orca;
using Orca.Authorization;
using Orca.Authorization.Abac;
using Orca.Authorization.Abac.Context;
using Orca.Authorization.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to add authorization services to an <see cref="IOrcaBuilder"/> instance.
/// </summary>
public static class OrcaBuilderExtensions
{
    /// <summary>
    /// Adds authorization services to the <see cref="IOrcaBuilder"/> with default configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to add authorization services to.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance, with authorization services added.</returns>
    public static IOrcaBuilder AddAuthorization(this IOrcaBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        AddAuthorization(builder, _ => { });

        return builder;
    }

    /// <summary>
    /// Adds authorization services to the <see cref="IOrcaBuilder"/> with custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IOrcaBuilder"/> to add authorization services to.</param>
    /// <param name="options">An action to configure <see cref="OrcaAuthorizationOptions"/>.</param>
    /// <returns>The <see cref="IOrcaBuilder"/> instance, with authorization services and custom configuration added.</returns>
    public static IOrcaBuilder AddAuthorization(this IOrcaBuilder builder, Action<OrcaAuthorizationOptions> options)
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
