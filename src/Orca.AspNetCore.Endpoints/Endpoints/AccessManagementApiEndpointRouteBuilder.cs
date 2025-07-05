namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Defines the root route builder for access control API endpoints.
/// </summary>
public partial class AccessManagementApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpoints;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessManagementApiEndpointRouteBuilder"/> class.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the access control routes.</param>
    public AccessManagementApiEndpointRouteBuilder(IEndpointRouteBuilder endpoints)
    {
        _endpoints = endpoints;
    }
}
