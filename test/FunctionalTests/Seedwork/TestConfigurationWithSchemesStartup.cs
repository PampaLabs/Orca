using Acheve.AspNetCore.TestHost.Security;
using Balea;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FunctionalTests.Seedwork
{
    public class TestConfigurationWithSchemesStartup
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
