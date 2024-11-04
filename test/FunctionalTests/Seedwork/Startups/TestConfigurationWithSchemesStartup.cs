using Acheve.AspNetCore.TestHost.Security;
using Balea;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FunctionalTests.Seedwork
{
    public class TestConfigurationWithSchemesStartup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddBalea(options =>
                {
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(ClaimTypes.Upn);
                })
                .AddInMemoryStores()
                .AddAuthorization(options =>
                {
                    options.Schemes.Add("scheme2");
                });

            services
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = "scheme1";
                    setup.DefaultChallengeScheme = "scheme1";
                })
                .AddTestServer("scheme1")
                .AddTestServer("scheme2", options =>
                {
                    options.RoleClaimType = "sourceRole";
                })
                .AddTestServer("scheme3");

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.Custom, builder =>
                    {
                        builder.RequireAuthenticatedUser();
                        builder.AddAuthenticationSchemes("scheme3");
                    });
                })
                .AddMvc();

            services.AddSingleton<IAsyncTestServerLifetime>(new AsyncTestServerLifetime
            {
                OnSetUpAsync = server => server.Host.ImportConfigurationAsync()
            });

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
