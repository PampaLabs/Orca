using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Balea;
using Balea.AspNetCore.Endpoints;

namespace Microsoft.AspNetCore.Routing;

using static EndpointRouteBuilderHelper;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
/// </summary>
public static class DelegationApiEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Add endpoints for registering, logging in, and logging out using ASP.NET Core Identity.
    /// </summary>
    /// <param name="endpoints">
    /// The <see cref="IEndpointRouteBuilder"/> to add the identity endpoints to.
    /// Call <see cref="EndpointRouteBuilderExtensions.MapGroup(IEndpointRouteBuilder, string)"/> to add a prefix to all the endpoints.
    /// </param>
    /// <param name="builder">
    /// </param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further customize the added endpoints.</returns>
    public static IEndpointConventionBuilder MapDelegationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var requestMapper = new DelegationRequestMapper();
        var responseMapper = new DelegationResponseMapper();

        var routeGroup = endpoints.MapGroup("/delegations");

        routeGroup.MapGet("", async Task<Results<Ok<IEnumerable<DelegationResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] DelegationFilter filter) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegations = await delegationStore.SearchAsync(filter);
            var response = delegations.Select(delegation => responseMapper.FromEntity(delegation));

            return TypedResults.Ok(response);
        });

        routeGroup.MapPost("", async Task<Results<Ok<DelegationResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] DelegationRequest data) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = requestMapper.ToEntity(data);

            var result = await delegationStore.CreateAsync(delegation);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = responseMapper.FromEntity(delegation);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}", async Task<Results<Ok<DelegationResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindByIdAsync(id);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(delegation);

            return TypedResults.Ok(response);
        });

        routeGroup.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] DelegationRequest data) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindByIdAsync(id);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            requestMapper.ToEntity(data, delegation);

            var result = await delegationStore.UpdateAsync(delegation);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
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

        routeGroup.MapGet("/subject/{subject}", async Task<Results<Ok<DelegationResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string subject) =>
        {
            var delegationStore = sp.GetRequiredService<IDelegationStore>();

            var delegation = await delegationStore.FindBySubjectAsync(subject);

            if (delegation is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(delegation);

            return TypedResults.Ok(response);
        });

        return new DelegationEndpointsConventionBuilder(routeGroup);
    }

    // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
    private sealed class DelegationEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }
}
