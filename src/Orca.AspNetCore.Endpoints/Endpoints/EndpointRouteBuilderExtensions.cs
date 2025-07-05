using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add access management endpoints.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps access management endpoints with the default configuration.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the routes.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further configure the route.</returns>
    public static IEndpointConventionBuilder MapAccessManagementApi(this IEndpointRouteBuilder endpoints)
    {
        return MapAccessManagementApi(endpoints, options =>
        {
            options.MapDelegationEndpoints();
            options.MapPermissionEndpoints();
            options.MapPolicyEndpoints();
            options.MapRoleEndpoints();
            options.MapSubjectEndpoints();
        });
    }

    /// <summary>
    /// Maps access management endpoints with a custom configuration.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the routes.</param>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further configure the route.</returns>
    public static IEndpointConventionBuilder MapAccessManagementApi(this IEndpointRouteBuilder endpoints, Action<AccessManagementApiEndpointRouteBuilder> optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var routeGroup = endpoints.MapGroup("").RequireAuthorization();

        var builder = new AccessManagementApiEndpointRouteBuilder(routeGroup);
        optionsBuilder.Invoke(builder);

        return routeGroup;
    }
}