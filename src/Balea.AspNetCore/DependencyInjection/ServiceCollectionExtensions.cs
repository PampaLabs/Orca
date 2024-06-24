using Balea;
using Balea.Abstractions;
using Balea.Authorization;
using Balea.Authorization.Abac;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IBaleaBuilder AddBalea(this IServiceCollection services, Action<BaleaAspNetOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure(setup);

            return services.AddBalea();
        }

        public static IBaleaBuilder AddBalea(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            //add balea required services
            services.AddAuthorization();
            services.AddHttpContextAccessor();
    		services.AddAppContextAccessor();

    		services.AddSingleton(sp => sp.GetRequiredService<IOptions<BaleaAspNetOptions>>().Value.Common);
    		services.AddSingleton(sp => sp.GetRequiredService<IOptions<BaleaAspNetOptions>>().Value.WebHost);
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddScoped<IAuthorizationGrantor, DefaultAuthorizationGrantor>();
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();

            services.AddScoped<IAspNetPropertyBag, UserPropertyBag>();
            services.AddScoped<IAspNetPropertyBag, ResourcePropertyBag>();
            services.AddScoped<IAspNetPropertyBag, ParameterPropertyBag>();
            services.AddScoped<AbacAuthorizationContextFactory>();

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, AbacAuthorizationHandler>();
            services.AddTransient<IPolicyEvaluator, BaleaPolicyEvaluator>();

            services.AddScoped<IAccessControlContext, AccessControlContext>();

            return new BaleaBuilder(services);
        }

    	public static IServiceCollection AddAppContextAccessor(this IServiceCollection services)
    	{
    		services.TryAddSingleton<IAppContextAccessor, AppContextAccessor>();
    		return services;
    	}
    }
}
