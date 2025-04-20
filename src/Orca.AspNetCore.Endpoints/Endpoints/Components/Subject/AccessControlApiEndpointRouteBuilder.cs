using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public partial class AccessControlApiEndpointRouteBuilder
{
    /// <summary>
    /// Maps subject endpoints with the default configuration.
    /// </summary>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapSubjectEndpoints()
    {
        return MapSubjectEndpoints(options =>
        {
            options.MapCreateEndpoint();
            options.MapReadEndpoint();
            options.MapUpdateEndpoint();
            options.MapDeleteEndpoint();
            options.MapListEndpoint();

            options.MapFindBySubEndpoint();

            options.MapGetRolesEndpoint();
            options.MapAddRoleEndpoint();
            options.MapRemoveRoleEndpoint();
        });
    }

    /// <summary>
    /// Maps subject endpoints with a custom configuration.
    /// </summary>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapSubjectEndpoints(Action<SubjectApiEndpointRouteBuilder> optionsBuilder)
    {
        var routeGroup = _endpoints.MapGroup("/subjects");

        var builder = new SubjectApiEndpointRouteBuilder(routeGroup);
        optionsBuilder?.Invoke(builder);

        return routeGroup;
    }
}
