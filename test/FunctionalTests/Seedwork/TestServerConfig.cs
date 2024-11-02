namespace FunctionalTests.Seedwork;

public static class TestServerConfig
{
    public static TestServerDescriptor[] Servers { get; } =
        [
            (typeof(TestConfigurationStartup), false), // Not support schemes
            (typeof(TestEntityFrameworkCoreStartup), false), // Not support schemes
            (typeof(TestConfigurationWithSchemesStartup), true) // Support schemes
        ];
}
