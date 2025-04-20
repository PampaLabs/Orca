namespace Orca.AspNetCore.Endpoints;

internal class RoleResponseMapper : IDataMapper<Role, RoleResponse>
{
    public void FromEntity(Role source, RoleResponse destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }

    public void ToEntity(RoleResponse source, Role destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }
}
