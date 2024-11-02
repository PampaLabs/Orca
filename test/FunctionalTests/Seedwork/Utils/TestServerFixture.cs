using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace FunctionalTests.Seedwork
{
    public class TestServerFixture : IAsyncLifetime
    {
        private List<TestServerInfo> _servers = [];

        public TestServer GetTestServer(Type startupType)
        {
            return _servers.Single(item => item.StartupType == startupType).TestServerInstance;
        }

        public async Task InitializeAsync()
        {
            _servers = GenerateTestServers().ToList();

            foreach (var server in _servers)
            {
                await server.TestServerInstance.SetUpAsync();
            }
        }

        public async Task DisposeAsync()
        {
            foreach (var server in _servers)
            {
                await server.TestServerInstance.TearDownAsync();
            }
        }

        public async Task CleanUpAsync()
        {
            foreach (var server in _servers)
            {
                await server.TestServerInstance.CleanUpAsync();
            }
        }

        private static IEnumerable<TestServerInfo> GenerateTestServers()
        {
            var startupTypes = TestServerConfig.Servers.Select(server => server.StartupType);

            foreach (var startupType in startupTypes)
            {
                var testServer = TestServerFactory.Create(startupType);
                var testServerInfo = (startupType, testServer);

                yield return testServerInfo;
            }
        }
    }
}
