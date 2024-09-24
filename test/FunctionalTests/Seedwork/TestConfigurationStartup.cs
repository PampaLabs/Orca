using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Balea;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FunctionalTests.Seedwork
{
    public class TestConfigurationStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddBalea(options =>
                {
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(ClaimTypes.Upn);
                })
                .AddConfigurationStore()
                .AddAuthorization();

            services
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer(options =>
                {
                    options.RoleClaimType = "sourceRole";
                });

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.Custom, builder =>
                    {
                        builder.RequireAuthenticatedUser();
                    });
                })
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
