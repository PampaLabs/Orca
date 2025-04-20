using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public partial class AccessControlApiEndpointRouteBuilder
{
    /// <summary>
    /// Maps permission endpoints with the default configuration.
    /// </summary>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapPermissionEndpoints()
    {
        return MapPermissionEndpoints(options =>
        {
            options.MapCreateEndpoint();
            options.MapReadEndpoint();
            options.MapUpdateEndpoint();
            options.MapDeleteEndpoint();
            options.MapListEndpoint();

            options.MapFindByNameEndpoint();

            options.MapGetRolesEndpoint();
            options.MapAddRoleEndpoint();
            options.MapRemoveRoleEndpoint();
        });
    }

    /// <summary>
    /// Maps permission endpoints with a custom configuration.
    /// </summary>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapPermissionEndpoints(Action<PermissionApiEndpointRouteBuilder> optionsBuilder)
    {
        var routeGroup = _endpoints.MapGroup("/permissions");

        var builder = new PermissionApiEndpointRouteBuilder(routeGroup);
        optionsBuilder?.Invoke(builder);

        return routeGroup;
    }
}
