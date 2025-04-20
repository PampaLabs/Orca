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
/// Defines the route mappings for API endpoints related to subject management.
/// </summary>
public class SubjectApiEndpointRouteBuilder
{
    private readonly IEndpointRouteBuilder _endpointRoute;

    private readonly SubjectDataMapper _mapper = new();

    /// <summary>
    /// Initializes a new instance of <see cref="SubjectApiEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpointRoute">The endpoint route builder used to map the routes.</param>
    public SubjectApiEndpointRouteBuilder(IEndpointRouteBuilder endpointRoute)
    {
        _endpointRoute = endpointRoute;
    }

    /// <summary>
    /// Maps the endpoint for listing subjects.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapListEndpoint()
    {
        return _endpointRoute.MapGet("", async Task<Results<Ok<IEnumerable<SubjectResponse>>, NotFound>>
            ([FromServices] IServiceProvider sp, [AsParameters] SubjectFilter filter) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subjects = await subjectStore.SearchAsync(filter);
            var response = subjects.Select(user => _mapper.ToResponse(user));

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint for creating a new subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapCreateEndpoint()
    {
        return _endpointRoute.MapPost("", async Task<Results<Ok<SubjectResponse>, ValidationProblem>>
            ([FromServices] IServiceProvider sp, [FromBody] SubjectRequest data) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subjects = _mapper.FromRequest(data);

            var result = await subjectStore.CreateAsync(subjects);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            var response = _mapper.ToResponse(subjects);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint for retrieving a subject by its ID.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapReadEndpoint()
    {
        return _endpointRoute.MapGet("/{id}", async Task<Results<Ok<SubjectResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(subject);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint for updating an existing subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapUpdateEndpoint()
    {
        return _endpointRoute.MapPut("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id, [FromBody] SubjectRequest data) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            _mapper.FromRequest(data, subject);

            var result = await subjectStore.UpdateAsync(subject);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }

    /// <summary>
    /// Maps the endpoint for deleting a subject by its ID.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapDeleteEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint for retrieving a subject by its sub value.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapFindBySubEndpoint()
    {
        return _endpointRoute.MapGet("/sub/{sub}", async Task<Results<Ok<SubjectResponse>, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string sub) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var subject = await subjectStore.FindBySubAsync(sub);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var response = _mapper.ToResponse(subject);

            return TypedResults.Ok(response);
        });
    }

    /// <summary>
    /// Maps the endpoint for retrieving roles associated with a subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapGetRolesEndpoint()
    {
        return _endpointRoute.MapGet("/{id}/roles", async Task<Results<Ok<RoleResponse[]>, ValidationProblem, NotFound>>
            ([FromServices] IServiceProvider sp, [FromRoute] string id) =>
        {
            var subjectStore = sp.GetRequiredService<ISubjectStore>();

            var roleMapper = new RoleDataMapper();

            var subject = await subjectStore.FindByIdAsync(id);

            if (subject is null)
            {
                return TypedResults.NotFound();
            }

            var roles = await subjectStore.GetRolesAsync(subject);
            var result = roles.Select(roleMapper.ToResponse);

            return TypedResults.Ok(result.ToArray());
        });
    }

    /// <summary>
    /// Maps the endpoint for assigning a role to a subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapAddRoleEndpoint()
    {
        return _endpointRoute.MapPost("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }

    /// <summary>
    /// Maps the endpoint for removing a role from a subject.
    /// </summary>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public RouteHandlerBuilder MapRemoveRoleEndpoint()
    {
        return _endpointRoute.MapDelete("/{id}/roles/{roleId}", async Task<Results<Ok, ValidationProblem, NotFound>>
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
    }
}
