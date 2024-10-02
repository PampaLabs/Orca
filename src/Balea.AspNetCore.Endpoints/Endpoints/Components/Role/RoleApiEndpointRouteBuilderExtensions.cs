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
public static class RoleApiEndpointRouteBuilderExtensions
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
    public static IEndpointConventionBuilder MapRoleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var requestMapper = new RoleRequestMapper();
        var responseMapper = new RoleResponseMapper();

        var routeGroup = endpoints.MapGroup("/roles");

        routeGroup.MapGet("", async Task<Results<Ok<IEnumerable<RoleResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] RoleFilter filter) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var roles = await roleStore.SearchAsync(filter);
            var response = roles.Select(role => responseMapper.FromEntity(role));

            return TypedResults.Ok(response);
        });

        routeGroup.MapPost("", async Task<Results<Ok<RoleResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] RoleRequest data) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = requestMapper.ToEntity(data);

            var result = await roleStore.CreateAsync(role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = responseMapper.FromEntity(role);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}", async Task<Results<Ok<RoleResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(role);

            return TypedResults.Ok(response);
        });

        routeGroup.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] RoleRequest data) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            requestMapper.ToEntity(data, role);

            var result = await roleStore.UpdateAsync(role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapGet("/name/{name}", async Task<Results<Ok<RoleResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByNameAsync(name);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(role);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}/subjects", async Task<Results<Ok<string[]>, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var subjects = await roleStore.GetSubjectsAsync(role);

            return TypedResults.Ok(subjects.ToArray());
        });

        routeGroup.MapPost("/{id}/subjects/{sub}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string sub) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.AddSubjectAsync(role, sub);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}/subjects/{sub}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string sub) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.RemoveSubjectAsync(role, sub);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapGet("/{id}/permission", async Task<Results<Ok<PermissionResponse[]>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var permissionMapper = new PermissionResponseMapper();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await roleStore.GetPermissionsAsync(role);
            var result = roles.Select(permissionMapper.FromEntity);

            return TypedResults.Ok(result.ToArray());
        });

        routeGroup.MapPost("/{id}/permission/{permissionId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string permissionId) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var role = await roleStore.FindByIdAsync(id);
            var permission = await permissionStore.FindByIdAsync(permissionId);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.AddPermissionAsync(role, permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}/permission/{permissionId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string permissionId) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var role = await roleStore.FindByIdAsync(id);
            var permission = await permissionStore.FindByIdAsync(permissionId);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.RemovePermissionAsync(role, permission);

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
