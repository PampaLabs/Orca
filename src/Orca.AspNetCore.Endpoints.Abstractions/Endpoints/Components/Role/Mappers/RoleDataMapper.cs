namespace Orca.AspNetCore.Endpoints;

internal class RoleDataMapper : EndpointDataMapper<Role, RoleRequest, RoleResponse>
{
    protected override IDataMapper<Role, RoleRequest> Request { get; }

    protected override IDataMapper<Role, RoleResponse> Response { get; }

    public RoleDataMapper()
    {
        Request = new RoleRequestMapper();
        Response = new RoleResponseMapper();
    }
}
