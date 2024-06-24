using Balea;
using Balea.Store.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using Testcontainers.MsSql;
using Xunit;

namespace FunctionalTests.Seedwork
{
    public class TestServerFixture : IAsyncLifetime
    {
        private static readonly Checkpoint _checkpoint = new()
        {
            TablesToIgnore = ["__EFMigrationsHistory"],
            WithReseed = true
        };

        private readonly MsSqlContainer _container = new MsSqlBuilder().Build();

        private readonly Dictionary<Type, IWebHost> _hosts = [];
        private readonly Dictionary<Type, TestServerInfo> _servers = [];

        public IReadOnlyCollection<TestServerInfo> Servers => _servers.Values;

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await InitializeTestServer();
        }

        public async Task DisposeAsync()
        {
            await _container.StopAsync();
        }

        private async Task InitializeTestServer()
        {
            var startups = new (Type TestServerType, bool SupportSchemas)[]
            {
                (typeof(TestConfigurationStartup), false), // Not support schemes
                (typeof(TestEntityFrameworkCoreStartup), false), // Not support schemes
                (typeof(TestConfigurationWithSchemesStartup), true) // Support schemes
            };

            foreach (var startup in startups)
            {
                var host = new WebHostBuilder()
                    .UseStartup(startup.TestServerType)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<IServer>(serviceProvider => new TestServer(serviceProvider));
                        services.AddSingleton(_container);
                    })
                    .ConfigureAppConfiguration(configure =>
                    {
                        CreateTestConfiguration(configure);
                    })
                    .Build();

                await host.StartAsync();
                await host.MigrateDbContextAsync<BaleaDbContext>();

                _hosts.Add(startup.TestServerType, host);
                _servers.Add(startup.TestServerType, new TestServerInfo(host.GetTestServer(), startup.SupportSchemas));
            }
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> func)
        {
            using (var scope = _hosts[typeof(TestEntityFrameworkCoreStartup)]
                .Services
                .GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                await func(scope.ServiceProvider);
            }
        }

        public async Task ExecuteDbContextAsync(Func<IAccessControlContext, Task> func)
        {
            await ExecuteScopeAsync(sp => func(sp.GetRequiredService<IAccessControlContext>()));
        }

        public async Task ResetDatabase()
        {
            await _checkpoint.Reset(_container.GetConnectionString());
            await _hosts[typeof(TestEntityFrameworkCoreStartup)].SeedDbContextAsync<BaleaDbContext>();
        }

        private static IConfigurationBuilder CreateTestConfiguration(IConfigurationBuilder builder)
        {
            return builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("balea.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }

    public class TestServerInfo
    {
        public TestServerInfo(TestServer testServer, bool supportSchemes)
        {
            TestServer = testServer;
            SupportSchemes = supportSchemes;
        }

        public TestServer TestServer { get; }

        public bool SupportSchemes { get; }
    }
}
