using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionalTests.Seedwork
{
    internal static class TestServerFactory
    {
        public static WebApplication Create(Type startupType)
        {
            var startup = Activator.CreateInstance(startupType) as IStartup;

            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("orca.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services
                .AddControllers()
                .AddApplicationPart(typeof(SchoolController).Assembly);

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            app.MapControllers();
            // startup.Configure(app);

            return app;
        }
    }
}