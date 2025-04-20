using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Orca;
using Orca.AspNetCore.Endpoints;

namespace Microsoft.AspNetCore.Routing;

using static EndpointRouteBuilderHelper;

/// <summary>
/// Defines the route mappings for API endpoints related to policy management.
/// </summary>
public class PolicyApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpointRoute;

    private readonly PolicyDataMapper _mapper = new();

    /// <summary>
    /// Initializes a new instance of <see cref="PolicyApiEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpointRoute">The endpoint route builder used to map the routes.</param>
    public PolicyApiEndpointRouteBuilder(IEndpointRouteBuilder endpointRoute)
    {
        _endpointRoute = endpointRoute;
    }

    /// <summary>
    /// Maps the endpoint to list policies.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapListEndpoint()
    {
        return _endpointRoute.MapGet("", async Task<Results<Ok<IEnumerable<PolicyResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] PolicyFilter filter) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policies = await policyStore.SearchAsync(filter);
            var response = policies.Select(permission => _mapper.ToResponse(permission));

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to create a new policy.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapCreateEndpoint()
    {
        return _endpointRoute.MapPost("", async Task<Results<Ok<PolicyResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] PolicyRequest data) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = _mapper.FromRequest(data);

            var result = await policyStore.CreateAsync(policy);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = _mapper.ToResponse(policy);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to get details of a specific policy.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapReadEndpoint()
    {
        return _endpointRoute.MapGet("/{id}", async Task<Results<Ok<PolicyResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByIdAsync(id);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(policy);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to update an existing policy.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapUpdateEndpoint()
    {
        return _endpointRoute.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] PolicyRequest data) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByIdAsync(id);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            _mapper.FromRequest(data, policy);

            var result = await policyStore.UpdateAsync(policy);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to delete a policy.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapDeleteEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByIdAsync(id);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            var result = await policyStore.DeleteAsync(policy);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to find a policy by name.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapFindByNameEndpoint()
    {
        return _endpointRoute.MapGet("/name/{name}", async Task<Results<Ok<PolicyResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByNameAsync(name);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(policy);

            return TypedResults.Ok(response);
        });
    }
}
