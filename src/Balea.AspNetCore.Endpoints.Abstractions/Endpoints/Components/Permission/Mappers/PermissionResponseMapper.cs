namespace Balea.AspNetCore.Endpoints;

internal class PermissionResponseMapper : IEntityMapper<Permission, PermissionResponse>
{
    public void FromEntity(Permission source, PermissionResponse destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
    }

    public void ToEntity(PermissionResponse source, Permission destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
    }
}
