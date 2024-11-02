using FunctionalTests.Seedwork;
using Xunit;

namespace FunctionalTests.Scenarios
{
    [Collection(nameof(TestServerCollectionFixture))]
    public abstract class ApiServerTest(TestServerFixture fixture) : IAsyncLifetime
    {
        protected TestServerFixture Fixture { get; } = fixture;

        public async Task InitializeAsync()
        {
            await Fixture.CleanUpAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
