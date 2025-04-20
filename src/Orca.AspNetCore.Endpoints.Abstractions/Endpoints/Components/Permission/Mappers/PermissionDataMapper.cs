namespace Orca.AspNetCore.Endpoints;

internal class PermissionDataMapper : EndpointDataMapper<Permission, PermissionRequest, PermissionResponse>
{
    protected override IDataMapper<Permission, PermissionRequest> Request { get; }

    protected override IDataMapper<Permission, PermissionResponse> Response { get; }

    public PermissionDataMapper()
    {
        Request = new PermissionRequestMapper();
        Response = new PermissionResponseMapper();
    }
}
