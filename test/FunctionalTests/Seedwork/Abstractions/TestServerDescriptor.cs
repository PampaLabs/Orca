namespace FunctionalTests.Seedwork;

public record class TestServerDescriptor(Type StartupType, bool SupportSchemas)
{
    public static implicit operator (Type StartupType, bool SupportSchemas)(TestServerDescriptor value)
    {
        return (value.StartupType, value.SupportSchemas);
    }

    public static implicit operator TestServerDescriptor((Type StartupType, bool SupportSchemas) value)
    {
        return new TestServerDescriptor(value.StartupType, value.SupportSchemas);
    }
}
