using Orca.Store.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Respawn;
using Testcontainers.MsSql;

namespace FunctionalTests.Seedwork;

public class EntityFrameworkTestServerLifetime : IAsyncTestServerLifetime
{
    private readonly MsSqlContainer _container;

    private Respawner _respawner;

    public EntityFrameworkTestServerLifetime(MsSqlContainer container)
    {
        _container = container;
    }

    public async Task SetUpAsync(TestServer server)
    {
        await _container.StartAsync();

        await server.Host.MigrateDbContextAsync<OrcaDbContext>();

        _respawner = await CreateCheckpointAsync();
    }

    public async Task CleanUpAsync(TestServer server)
    {
        var connectionString = _container.GetConnectionString();
        await _respawner.ResetAsync(connectionString);
    }

    public async Task TearDownAsync(TestServer server)
    {
        await _container.StopAsync();
    }

    private async Task<Respawner> CreateCheckpointAsync()
    {
        var connectionString = _container.GetConnectionString();
        var respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"],
            WithReseed = true
        });

        return respawner;
    }
}
