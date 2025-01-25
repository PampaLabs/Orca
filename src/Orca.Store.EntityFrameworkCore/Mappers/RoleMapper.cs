using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

internal class RoleMapper : IEntityMapper<RoleEntity, Role>
{
    public void FromEntity(RoleEntity source, Role destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = source.Mappings.Select(mapping => mapping.Mapping).ToList();
    }

    public void ToEntity(Role source, RoleEntity destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = source.Mappings.Select(mapping => new RoleMappingEntity() { Mapping = mapping }).ToList();
    }
}
