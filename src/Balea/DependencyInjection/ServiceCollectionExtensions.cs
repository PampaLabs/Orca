using Balea;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IBaleaBuilder AddBalea(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            AddBalea(services, _ => { });

            return new BaleaBuilder(services);
        }

        public static IBaleaBuilder AddBalea(this IServiceCollection services, Action<BaleaOptions> options)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            services.AddAccessControlContext();
            services.AddAppContextAccessor();

            services.AddScoped<IAuthorizationGrantor, DefaultAuthorizationGrantor>();
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();

            services.Configure(options);

            return new BaleaBuilder(services);
        }

        public static IServiceCollection AddAppContextAccessor(this IServiceCollection services)
    	{
    		services.TryAddSingleton<IAppContextAccessor, AppContextAccessor>();
    		return services;
    	}

        public static IServiceCollection AddAccessControlContext(this IServiceCollection services)
        {
            services.TryAddScoped<IAccessControlContext, AccessControlContext>();
            return services;
        }
    }
}
