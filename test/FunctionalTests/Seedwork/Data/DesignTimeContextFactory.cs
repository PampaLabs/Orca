using Orca.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FunctionalTests.Seedwork.Data
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<OrcaDbContext>
    {
        public OrcaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<OrcaDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                });

            return new OrcaDbContext(builder.Options);
        }
    }
}
