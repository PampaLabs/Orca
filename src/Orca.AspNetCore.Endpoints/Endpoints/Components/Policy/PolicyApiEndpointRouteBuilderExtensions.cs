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
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
/// </summary>
public static class PolicyApiEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Add endpoints for registering, logging in, and logging out using ASP.NET Core Identity.
    /// </summary>
    /// <param name="endpoints">
    /// The <see cref="IEndpointRouteBuilder"/> to add the identity endpoints to.
    /// Call <see cref="EndpointRouteBuilderExtensions.MapGroup(IEndpointRouteBuilder, string)"/> to add a prefix to all the endpoints.
    /// </param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further customize the added endpoints.</returns>
    public static IEndpointConventionBuilder MapPolicyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var requestMapper = new PolicyRequestMapper();
        var responseMapper = new PolicyResponseMapper();

        var routeGroup = endpoints.MapGroup("/policies");

        routeGroup.MapGet("", async Task<Results<Ok<IEnumerable<PolicyResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] PolicyFilter filter) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policies = await policyStore.SearchAsync(filter);
            var response = policies.Select(permission => responseMapper.FromEntity(permission));

            return TypedResults.Ok(response);
        });

        routeGroup.MapPost("", async Task<Results<Ok<PolicyResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] PolicyRequest data) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = requestMapper.ToEntity(data);

            var result = await policyStore.CreateAsync(policy);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = responseMapper.FromEntity(policy);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}", async Task<Results<Ok<PolicyResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByIdAsync(id);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(policy);

            return TypedResults.Ok(response);
        });

        routeGroup.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] PolicyRequest data) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByIdAsync(id);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            requestMapper.ToEntity(data, policy);

            var result = await policyStore.UpdateAsync(policy);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
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

        routeGroup.MapGet("/name/{name}", async Task<Results<Ok<PolicyResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var policyStore = sp.GetRequiredService<IPolicyStore>();

            var policy = await policyStore.FindByNameAsync(name);

            if (policy is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(policy);

            return TypedResults.Ok(response);
        });

        return new PolicyEndpointsConventionBuilder(routeGroup);
    }

    // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
    private sealed class PolicyEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }
}
