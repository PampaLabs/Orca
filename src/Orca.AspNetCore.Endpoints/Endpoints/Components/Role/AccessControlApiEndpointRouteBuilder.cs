using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public partial class AccessControlApiEndpointRouteBuilder
{
    /// <summary>
    /// Maps role endpoints with the default configuration.
    /// </summary>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapRoleEndpoints()
    {
        return MapRoleEndpoints(options =>
        {
            options.MapCreateEndpoint();
            options.MapReadEndpoint();
            options.MapUpdateEndpoint();
            options.MapDeleteEndpoint();
            options.MapListEndpoint();

            options.MapFindByNameEndpoint();

            options.MapGetSubjectsEndpoint();
            options.MapAddSubjectEndpoint();
            options.MapRemoveSubjectEndpoint();

            options.MapGetPermissionsEndpoint();
            options.MapAddPermissionEndpoint();
            options.MapRemovePermissionEndpoint();
        });
    }

    /// <summary>
    /// Maps role endpoints with a custom configuration.
    /// </summary>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapRoleEndpoints(Action<RoleApiEndpointRouteBuilder> optionsBuilder)
    {
        var routeGroup = _endpoints.MapGroup("/roles");

        var builder = new RoleApiEndpointRouteBuilder(routeGroup);
        optionsBuilder?.Invoke(builder);

        return routeGroup;
    }
}
