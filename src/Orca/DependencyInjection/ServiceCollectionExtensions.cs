using Orca;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IOrcaBuilder AddOrca(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            AddOrca(services, _ => { });

            return new OrcaBuilder(services);
        }

        public static IOrcaBuilder AddOrca(this IServiceCollection services, Action<OrcaOptions> options)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            services.AddAccessControlContext();

            services.AddScoped<IAuthorizationGrantor, DefaultAuthorizationGrantor>();
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();

            services.Configure(options);

            return new OrcaBuilder(services);
        }

        private static IServiceCollection AddAccessControlContext(this IServiceCollection services)
        {
            services.TryAddScoped<IAccessControlContext, AccessControlContext>();
            return services;
        }
    }
}
