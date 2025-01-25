using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FunctionalTests.Seedwork
{
    internal static class TestServerFactory
    {
        public static TestServer Create(Type startupType)
        {
            var host = new WebHostBuilder()
                .UseStartup(startupType)
                .ConfigureAppConfiguration(configuration =>
                {
                    configuration
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("orca.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                });

            return new TestServer(host);
        }
    }
}