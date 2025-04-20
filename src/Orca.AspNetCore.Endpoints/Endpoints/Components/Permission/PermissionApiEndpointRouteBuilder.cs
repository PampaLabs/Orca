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
/// Defines the route mappings for API endpoints related to permission management.
/// </summary>
public class PermissionApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpointRoute;

    private readonly PermissionDataMapper _mapper = new();

    /// <summary>
    /// Initializes a new instance of <see cref="PermissionApiEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpointRoute">The endpoint route builder used to map the routes.</param>
    public PermissionApiEndpointRouteBuilder(IEndpointRouteBuilder endpointRoute)
    {
        _endpointRoute = endpointRoute;
    }

    /// <summary>
    /// Maps the endpoint to list permissions.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapListEndpoint()
    {
        return _endpointRoute.MapGet("", async Task<Results<Ok<IEnumerable<PermissionResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] PermissionFilter filter) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permissions = await permissionStore.SearchAsync(filter);
            var response = permissions.Select(permission => _mapper.ToResponse(permission));

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to create a new permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapCreateEndpoint()
    {
        return _endpointRoute.MapPost("", async Task<Results<Ok<PermissionResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] PermissionRequest data) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = _mapper.FromRequest(data);

            var result = await permissionStore.CreateAsync(permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = _mapper.ToResponse(permission);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to get details of a specific permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapReadEndpoint()
    {
        return _endpointRoute.MapGet("/{id}", async Task<Results<Ok<PermissionResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(permission);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to update an existing permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapUpdateEndpoint()
    {
        return _endpointRoute.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] PermissionRequest data) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            _mapper.FromRequest(data, permission);

            var result = await permissionStore.UpdateAsync(permission);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to delete a permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapDeleteEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint to find a permission by name.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapFindByNameEndpoint()
    {
        return _endpointRoute.MapGet("/name/{name}", async Task<Results<Ok<PermissionResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var permission = await permissionStore.FindByNameAsync(name);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(permission);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to retrieve all roles associated with a permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapGetRolesEndpoint()
    {
        return _endpointRoute.MapGet("/{id}/roles", async Task<Results<Ok<RoleResponse[]>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var permissionStore = sp.GetRequiredService<IPermissionStore>();

            var roleMapper = new RoleDataMapper();

            var permission = await permissionStore.FindByIdAsync(id);

            if (permission is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await permissionStore.GetRolesAsync(permission);
            var result = roles.Select(roleMapper.ToResponse);

            return TypedResults.Ok(result.ToArray());
        });
    }

    /// <summary>
    /// Maps the endpoint to associate a role with a permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapAddRoleEndpoint()
    {
        return _endpointRoute.MapPost("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint to remove a role from a permission.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapRemoveRoleEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }
}
