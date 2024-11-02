using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests.Seedwork;

public class AsyncTestServerLifetime : IAsyncTestServerLifetime
{
    public Func<TestServer, Task> OnSetUpAsync { get; set; } = _ => Task.CompletedTask;

    public Func<TestServer, Task> OnCleanUpAsync { get; set; } = _ => Task.CompletedTask;

    public Func<TestServer, Task> OnTearDownAsync { get; set; } = _ => Task.CompletedTask;

    public Task SetUpAsync(TestServer server) => OnSetUpAsync(server);

    public Task CleanUpAsync(TestServer server) => OnCleanUpAsync(server);

    public Task TearDownAsync(TestServer server) => OnTearDownAsync(server);
}
