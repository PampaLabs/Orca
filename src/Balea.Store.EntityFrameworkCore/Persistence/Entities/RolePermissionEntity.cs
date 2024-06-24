namespace Balea.Store.EntityFrameworkCore.Entities;

public class RolePermissionEntity
{
    public string PermissionId { get; set; }
    public string RoleId { get; set; }

    public RoleEntity Role { get; set; }
    public PermissionEntity Permission { get; set; }
}
