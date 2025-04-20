namespace Orca.AspNetCore.Endpoints;

internal class RoleRequestMapper : IDataMapper<Role, RoleRequest>
{
    public void FromEntity(Role source, RoleRequest destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }

    public void ToEntity(RoleRequest source, Role destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }
}
