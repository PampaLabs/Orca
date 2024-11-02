using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests.Seedwork;

public interface IAsyncTestServerLifetime
{
    Task SetUpAsync(TestServer testServer);

    Task CleanUpAsync(TestServer testServer);

    Task TearDownAsync(TestServer testServer);
}
