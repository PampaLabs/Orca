using Orca;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to configure and add Orca services to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Orca services to the specified <see cref="IServiceCollection"/> with default options.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <returns>An instance of <see cref="IOrcaBuilder"/> for further configuration.</returns>
        public static IOrcaBuilder AddOrca(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            AddOrca(services, _ => { });

            return new OrcaBuilder(services);
        }

        /// <summary>
        /// Adds Orca services to the specified <see cref="IServiceCollection"/> and allows custom configuration.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <param name="options">A delegate to configure the options.</param>
        /// <returns>An instance of <see cref="IOrcaBuilder"/> for further configuration.</returns>
        public static IOrcaBuilder AddOrca(this IServiceCollection services, Action<OrcaOptions> options)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            services.AddAccessManagementContext();

            services.AddScoped<IAuthorizationGrantor, DefaultAuthorizationGrantor>();
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();

            services.Configure(options);

            return new OrcaBuilder(services);
        }

        private static IServiceCollection AddAccessManagementContext(this IServiceCollection services)
        {
            services.TryAddScoped<IOrcaStoreAccessor, OrcaStoreAccessor>();
            return services;
        }
    }
}
