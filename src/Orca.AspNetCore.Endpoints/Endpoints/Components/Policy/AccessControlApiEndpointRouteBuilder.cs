using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public partial class AccessControlApiEndpointRouteBuilder
{
    /// <summary>
    /// Maps policy endpoints with the default configuration.
    /// </summary>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapPolicyEndpoints()
    {
        return MapPolicyEndpoints(options =>
        {
            options.MapCreateEndpoint();
            options.MapReadEndpoint();
            options.MapUpdateEndpoint();
            options.MapDeleteEndpoint();
            options.MapListEndpoint();

            options.MapFindByNameEndpoint();
        });
    }

    /// <summary>
    /// Maps policy endpoints with a custom configuration.
    /// </summary>
    /// <param name="optionsBuilder">A delegate to configure which endpoints to include.</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> for further endpoint customization.</returns>
    public IEndpointConventionBuilder MapPolicyEndpoints(Action<PolicyApiEndpointRouteBuilder> optionsBuilder)
    {
        var routeGroup = _endpoints.MapGroup("/policies");

        var builder = new PolicyApiEndpointRouteBuilder(routeGroup);
        optionsBuilder?.Invoke(builder);

        return routeGroup;
    }
}
