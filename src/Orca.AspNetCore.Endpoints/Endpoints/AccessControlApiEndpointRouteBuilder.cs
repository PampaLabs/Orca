namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Defines the root route builder for access control API endpoints.
/// </summary>
public partial class AccessControlApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpoints;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessControlApiEndpointRouteBuilder"/> class.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to map the access control routes.</param>
    public AccessControlApiEndpointRouteBuilder(IEndpointRouteBuilder endpoints)
    {
        _endpoints = endpoints;
    }
}
