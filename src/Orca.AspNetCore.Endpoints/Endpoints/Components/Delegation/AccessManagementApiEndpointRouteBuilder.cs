using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public partial class AccessManagementApiEndpointRouteBuilder
{
    /// <summary>
    /// Maps delegation endpoints with the default configuration.
    /// </summary>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapDelegationEndpoints()
    {
        return MapDelegationEndpoints(options =>
        {
            options.MapCreateEndpoint();
            options.MapReadEndpoint();
            options.MapUpdateEndpoint();
            options.MapDeleteEndpoint();
            options.MapListEndpoint();

            options.MapFindBySubjectEndpoint();
        });
    }

    /// <summary>
    /// Maps delegation endpoints with a custom configuration.
    /// </summary>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapDelegationEndpoints(Action<DelegationApiEndpointRouteBuilder> optionsBuilder)
    {
        var routeGroup = _endpoints.MapGroup("/delegations");

        var builder = new DelegationApiEndpointRouteBuilder(routeGroup);
        optionsBuilder?.Invoke(builder);

        return routeGroup;
    }
}
