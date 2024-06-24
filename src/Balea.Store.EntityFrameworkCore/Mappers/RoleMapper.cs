using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

internal class RoleMapper : IEntityMapper<RoleEntity, Role>
{
    public void FromEntity(RoleEntity source, Role destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(Role source, RoleEntity destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
    }
}
