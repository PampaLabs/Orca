namespace Balea.Store.EntityFrameworkCore.Entities;

public class RoleMappingEntity
{
    public int Id { get; set; }
    public string Mapping { get; set; }
    public string RoleId { get; set; }
    public RoleEntity Role { get; set; }
}
