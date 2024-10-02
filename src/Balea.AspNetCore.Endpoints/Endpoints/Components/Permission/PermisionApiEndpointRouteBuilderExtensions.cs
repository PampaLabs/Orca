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
public static class PermissionApiEndpointRouteBuilderExtensions
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
    public static IEndpointConventionBuilder MapPermissionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var requestMapper = new PermissionRequestMapper();
        var responseMapper = new PermissionResponseMapper();

        var routeGroup = endpoints.MapGroup("/permissions");

        routeGroup.MapGet("", async Task<Results<Ok<IEnumerable<PermissionResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] PermissionFilter filter) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permissions = await permissionStore.SearchAsync(filter);
            var response = permissions.Select(permission => responseMapper.FromEntity(permission));

            return TypedResults.Ok(response);
        });

        routeGroup.MapPost("", async Task<Results<Ok<PermissionResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] PermissionRequest data) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = requestMapper.ToEntity(data);

            var result = await permissionStore.CreateAsync(permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = responseMapper.FromEntity(permission);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}", async Task<Results<Ok<PermissionResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(permission);

            return TypedResults.Ok(response);
        });

        routeGroup.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] PermissionRequest data) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            requestMapper.ToEntity(data, permission);

            var result = await permissionStore.UpdateAsync(permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var result = await permissionStore.DeleteAsync(permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapGet("/name/{name}", async Task<Results<Ok<PermissionResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByNameAsync(name);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var response = responseMapper.FromEntity(permission);

            return TypedResults.Ok(response);
        });

        routeGroup.MapGet("/{id}/roles", async Task<Results<Ok<RoleResponse[]>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var roleMapper = new RoleResponseMapper();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await permissionStore.GetRolesAsync(permission);
            var result = roles.Select(roleMapper.FromEntity);

            return TypedResults.Ok(result.ToArray());
        });

        routeGroup.MapPost("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string roleId) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var permission = await permissionStore.FindByIdAsync(id);
            var role = await roleStore.FindByIdAsync(roleId);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await permissionStore.AddRoleAsync(permission, role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        routeGroup.MapDelete("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string roleId) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var permission = await permissionStore.FindByIdAsync(id);
            var role = await roleStore.FindByIdAsync(roleId);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var result = await permissionStore.RemoveRoleAsync(permission, role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        return new PermissionEndpointsConventionBuilder(routeGroup);
    }

    // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
    private sealed class PermissionEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }
}
