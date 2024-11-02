namespace FunctionalTests.Seedwork;

public static class TestServerData
{
    private static TestServerDescriptor[] Collection => TestServerConfig.Servers;

    public static IEnumerable<object[]> GetTestServers()
    {
        return Collection.Select(item => new Type[] { item.StartupType });
    }

    public static IEnumerable<object[]> GetTestServersWithSchemaSupport()
    {
        return Collection
            .Where(item => item.SupportSchemas)
            .Select(item => new Type[] { item.StartupType });
    }

    public static IEnumerable<object[]> GetTestServersWithoutSchemaSupport()
    {
        return Collection
            .Where(item => !item.SupportSchemas)
            .Select(item => new Type[] { item.StartupType });
    }
}
