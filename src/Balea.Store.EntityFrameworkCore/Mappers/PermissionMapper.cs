using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

internal class PermissionMapper : IEntityMapper<PermissionEntity, Permission>
{
    public void FromEntity(PermissionEntity source, Permission destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
    }

    public void ToEntity(Permission source, PermissionEntity destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
    }
}
