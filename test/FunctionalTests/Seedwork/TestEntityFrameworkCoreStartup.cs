using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Balea;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Testcontainers.MsSql;

namespace FunctionalTests.Seedwork
{
    public class TestEntityFrameworkCoreStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddBalea(options =>
                {
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(ClaimTypes.Upn);
                })
                .AddEntityFrameworkCoreStore((sp, options) =>
                {
                    var container = sp.GetRequiredService<MsSqlContainer>();

                    options.UseSqlServer(container.GetConnectionString(), sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(typeof(TestEntityFrameworkCoreStartup).Assembly.FullName);
                    })
                    .UseLoggerFactory(LoggerFactory.Create(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Information).AddConsole();
                    }));
                })
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
