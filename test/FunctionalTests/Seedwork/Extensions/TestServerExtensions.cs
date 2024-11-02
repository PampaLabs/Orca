using FunctionalTests.Seedwork;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.TestHost;

public static class TestServerExtensions
{
    public static async Task SetUpAsync(this TestServer testServer)
    {
        var lifetime = testServer.Services.GetService<IAsyncTestServerLifetime>();

        if (lifetime is not null)
        {
            await lifetime.SetUpAsync(testServer);
        }
    }

    public static async Task CleanUpAsync(this TestServer testServer)
    {
        var lifetime = testServer.Services.GetService<IAsyncTestServerLifetime>();

        if (lifetime is not null)
        {
            await lifetime.CleanUpAsync(testServer);
        }
    }

    public static async Task TearDownAsync(this TestServer testServer)
    {
        var lifetime = testServer.Services.GetService<IAsyncTestServerLifetime>();

        if (lifetime is not null)
        {
            await lifetime.TearDownAsync(testServer);
        }
    }
}
