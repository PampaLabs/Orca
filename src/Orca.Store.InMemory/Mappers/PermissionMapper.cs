namespace Orca.Store.Configuration;

internal class PermissionMapper : IEntityMapper<PermissionConfiguration, Permission>
{
    public void FromEntity(PermissionConfiguration source, Permission destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
    }

    public void ToEntity(Permission source, PermissionConfiguration destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
    }
}
