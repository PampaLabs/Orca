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
/// Defines the route mappings for API endpoints related to role management.
/// </summary>
public class RoleApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpointRoute;

    private readonly RoleDataMapper _mapper = new();

    /// <summary>
    /// Initializes a new instance of <see cref="RoleApiEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpointRoute">The endpoint route builder used to map the routes.</param>
    public RoleApiEndpointRouteBuilder(IEndpointRouteBuilder endpointRoute)
    {
        _endpointRoute = endpointRoute;
    }

    /// <summary>
    /// Maps the endpoint to list roles.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapListEndpoint()
    {
        return _endpointRoute.MapGet("", async Task<Results<Ok<IEnumerable<RoleResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] RoleFilter filter) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var roles = await roleStore.SearchAsync(filter);
            var response = roles.Select(role => _mapper.ToResponse(role));

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to create a new role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapCreateEndpoint()
    {
        return _endpointRoute.MapPost("", async Task<Results<Ok<RoleResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] RoleRequest data) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = _mapper.FromRequest(data);

            var result = await roleStore.CreateAsync(role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = _mapper.ToResponse(role);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to get details of a specific role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapReadEndpoint()
    {
        return _endpointRoute.MapGet("/{id}", async Task<Results<Ok<RoleResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(role);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to update an existing role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapUpdateEndpoint()
    {
        return _endpointRoute.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] RoleRequest data) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            _mapper.FromRequest(data, role);

            var result = await roleStore.UpdateAsync(role);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to delete a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapDeleteEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint to find a role by its name.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapFindByNameEndpoint()
    {
        return _endpointRoute.MapGet("/name/{name}", async Task<Results<Ok<RoleResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string name) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var role = await roleStore.FindByNameAsync(name);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(role);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint to get the subjects assigned to a specific role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapGetSubjectsEndpoint()
    {
        return _endpointRoute.MapGet("/{id}/subjects", async Task<Results<Ok<SubjectResponse[]>, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var subjectMapper = new SubjectDataMapper();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var subjects = await roleStore.GetSubjectsAsync(role);
            var result = subjects.Select(subjectMapper.ToResponse);

            return TypedResults.Ok(result.ToArray());
        });
    }

    /// <summary>
    /// Maps the endpoint to assign a subject to a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapAddSubjectEndpoint()
    {
        return _endpointRoute.MapPost("/{id}/subjects/{subjectId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string subjectId) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var role = await roleStore.FindByIdAsync(id);
            var subject = await subjectStore.FindByIdAsync(subjectId);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.AddSubjectAsync(role, subject);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to remove a subject from a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapRemoveSubjectEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}/subjects/{subjectId}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromRoute] string subjectId) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var role = await roleStore.FindByIdAsync(id);
            var subject = await subjectStore.FindByIdAsync(subjectId);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var result = await roleStore.RemoveSubjectAsync(role, subject);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint to get the permissions associated with a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapGetPermissionsEndpoint()
    {
        return _endpointRoute.MapGet("/{id}/permissions", async Task<Results<Ok<PermissionResponse[]>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var roleStore = sp.GetRequiredService<IRoleStore>();

            var permissionMapper = new PermissionDataMapper();

            var role = await roleStore.FindByIdAsync(id);

            if (role is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await roleStore.GetPermissionsAsync(role);
            var result = roles.Select(permissionMapper.ToResponse);

            return TypedResults.Ok(result.ToArray());
        });
    }

    /// <summary>
    /// Maps the endpoint to assign a permission to a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapAddPermissionEndpoint()
    {
        return _endpointRoute.MapPost("/{id}/permissions/{permissionId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint to remove a permission from a role.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapRemovePermissionEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}/permissions/{permissionId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }
}
