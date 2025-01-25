using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Orca;
using Orca.Store.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Testcontainers.MsSql;

namespace FunctionalTests.Seedwork
{
    public class TestEntityFrameworkCoreStartup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrcaDbContext>((sp, options) =>
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
            });

            services
                .AddOrca(options =>
                {
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
                    options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(ClaimTypes.Upn);
                })
                .AddEntityFrameworkStores()
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

            var container = new MsSqlBuilder().Build();

            services.AddSingleton(container);
            services.AddSingleton<IAsyncTestServerLifetime, EntityFrameworkTestServerLifetime>();

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
