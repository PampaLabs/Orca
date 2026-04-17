using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests.Seedwork;

public record class TestServerInfo(Type StartupType, TestServer TestServerInstance, WebApplication WepApp)
{
    public static implicit operator (Type TestServerType, TestServer TestServerInstance, WebApplication WepApp)(TestServerInfo value)
    {
        return (value.StartupType, value.TestServerInstance, value.WepApp);
    }

    public static implicit operator TestServerInfo((Type StartupType, TestServer TestServerInstance, WebApplication WepApp) value)
    {
        return new TestServerInfo(value.StartupType, value.TestServerInstance, value.WepApp);
    }
}
