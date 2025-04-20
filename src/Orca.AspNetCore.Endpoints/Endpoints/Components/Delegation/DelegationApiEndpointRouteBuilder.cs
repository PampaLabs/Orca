using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Orca;
using Orca.AspNetCore.Endpoints;

namespace Microsoft.AspNetCore.Routing;

using static EndpointRouteBuilderHelper;

/// <summary>
/// Defines the route mappings for API endpoints related to delegation management.
/// </summary>
public class DelegationApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpointRoute;

    private readonly DelegationDataMapper _mapper = new();

    /// <summary>
    /// Initializes a new instance of <see cref="DelegationApiEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpointRoute">The endpoint route builder used to map the routes.</param>
    public DelegationApiEndpointRouteBuilder(IEndpointRouteBuilder endpointRoute)
    {
        _endpointRoute = endpointRoute;
    }

    /// <summary>
    /// Maps the endpoint to list delegations.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapListEndpoint()
    {
        return _endpointRoute.MapGet("", async Task<Results<Ok<IEnumerable<DelegationResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] DelegationFilter filter) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegations = await delegationStore.SearchAsync(filter);
            var response = delegations.Select(delegation => _mapper.ToResponse(delegation));

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to create a new delegation.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapCreateEndpoint()
    {
        return _endpointRoute.MapPost("", async Task<Results<Ok<DelegationResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] DelegationRequest data) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = _mapper.FromRequest(data);

            var result = await delegationStore.CreateAsync(delegation);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = _mapper.ToResponse(delegation);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to get details of a specific delegation.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapReadEndpoint()
    {
        return _endpointRoute.MapGet("/{id}", async Task<Results<Ok<DelegationResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindByIdAsync(id);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(delegation);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to update an existing delegation.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapUpdateEndpoint()
    {
        return _endpointRoute.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] DelegationRequest data) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindByIdAsync(id);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            _mapper.FromRequest(data, delegation);

            var result = await delegationStore.UpdateAsync(delegation);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to delete a delegation.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapDeleteEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindByIdAsync(id);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            var result = await delegationStore.DeleteAsync(delegation);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to find a delegation by subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapFindBySubjectEndpoint()
    {
        return _endpointRoute.MapGet("/subject/{subject}", async Task<Results<Ok<DelegationResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string subject) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindBySubjectAsync(subject);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(delegation);

            return TypedResults.Ok(response);
        });
    }
}
