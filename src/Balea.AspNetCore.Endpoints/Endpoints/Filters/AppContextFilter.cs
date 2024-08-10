using Balea;
using Balea.Store.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

internal class AppContextFilter : IEndpointFilter
{
    private readonly IAppContextAccessor _contextAccessor;

    public AppContextFilter(IAppContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        if (httpContext.Request.Headers.TryGetValue(HttpStoreDefaults.AppContextHeader, out var application))
        {
            _contextAccessor.AppContext.Name = application;
        }

        return await next(context);
    }
}

internal static partial class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder AddAppContextFilter(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<AppContextFilter>();
    }

    public static RouteGroupBuilder AddAppContextFilter(this RouteGroupBuilder builder)
    {
        return builder.AddEndpointFilter<AppContextFilter>();
    }
}
