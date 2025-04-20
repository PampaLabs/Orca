using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add access control endpoints.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps access control endpoints with the default configuration.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the routes.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further configure the route.</returns>
    public static IEndpointConventionBuilder MapAccessControlApi(this IEndpointRouteBuilder endpoints)
    {
        return MapAccessControlApi(endpoints, options =>
        {
            options.MapDelegationEndpoints();
            options.MapPermissionEndpoints();
            options.MapPolicyEndpoints();
            options.MapRoleEndpoints();
            options.MapSubjectEndpoints();
        });
    }

    /// <summary>
    /// Maps access control endpoints with a custom configuration.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the routes.</param>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further configure the route.</returns>
    public static IEndpointConventionBuilder MapAccessControlApi(this IEndpointRouteBuilder endpoints, Action<AccessControlApiEndpointRouteBuilder> optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var routeGroup = endpoints.MapGroup("").RequireAuthorization();

        var builder = new AccessControlApiEndpointRouteBuilder(routeGroup);
        optionsBuilder.Invoke(builder);

        return routeGroup;
    }
}