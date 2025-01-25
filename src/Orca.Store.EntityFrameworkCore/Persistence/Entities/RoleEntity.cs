namespace Orca.Store.EntityFrameworkCore.Entities;

public class RoleEntity
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public bool Enabled { get; set; } = true;

	public List<RoleMappingEntity> Mappings { get; set; } = [];
}
