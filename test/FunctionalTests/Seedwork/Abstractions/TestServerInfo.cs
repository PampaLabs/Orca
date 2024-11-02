using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests.Seedwork;

public record class TestServerInfo(Type StartupType, TestServer TestServerInstance)
{
    public static implicit operator (Type TestServerType, TestServer TestServerInstance)(TestServerInfo value)
    {
        return (value.StartupType, value.TestServerInstance);
    }

    public static implicit operator TestServerInfo((Type StartupType, TestServer TestServerInstance) value)
    {
        return new TestServerInfo(value.StartupType, value.TestServerInstance);
    }
}
