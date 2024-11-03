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
public static class SubjectApiEndpointRouteBuilderExtensions
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
    public static IEndpointConventionBuilder MapSubjectEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var requestMapper = new SubjectRequestMapper();
        var responseMapper = new SubjectResponseMapper();

        var routeGroup = endpoints.MapGroup("/subjects");

        routeGroup.MapGet("", async Task<Results<Ok<IEnumerable<SubjectResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] SubjectFilter filter) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subjects = await subjectStore.SearchAsync(filter);
            var response = subjects.Select(user => responseMapper.FromEntity(user));

            return TypedResults.Ok(response);
        });

        routeGroup.MapPost("", async Task<Results<Ok<SubjectResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] SubjectRequest data) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subjects = requestMapper.ToEntity(data);

            var result = await subjectStore.CreateAsync(subjects);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = responseMapper.FromEntity(subjects);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}", async Task<Results<Ok<SubjectResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(subject);

            return TypedResults.Ok(response);
        });

        routeGroup.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] SubjectRequest data) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            requestMapper.ToEntity(data, subject);

            var result = await subjectStore.UpdateAsync(subject);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var result = await subjectStore.DeleteAsync(subject);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapGet("/sub/{subject}", async Task<Results<Ok<SubjectResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string sub) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindBySubAsync(sub);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(subject);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}/roles", async Task<Results<Ok<RoleResponse[]>, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var roleMapper = new RoleResponseMapper();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await subjectStore.GetRolesAsync(subject);
            var result = roles.Select(roleMapper.FromEntity);

            return TypedResults.Ok(result.ToArray());
        });

        routeGroup.MapPost("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string roleId) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var subject = await subjectStore.FindByIdAsync(id);
            var role = await roleStore.FindByIdAsync(roleId);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await subjectStore.AddRoleAsync(subject, role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string roleId) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var subject = await subjectStore.FindByIdAsync(id);
            var role = await roleStore.FindByIdAsync(roleId);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await subjectStore.RemoveRoleAsync(subject, role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        return new RoleEndpointsConventionBuilder(routeGroup);
    }

    // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
    private sealed class RoleEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }
}
