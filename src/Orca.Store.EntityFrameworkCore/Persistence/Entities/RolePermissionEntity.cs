namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a role permission entity.
/// </summary>
public class RolePermissionEntity
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    public string PermissionId { get; set; }

    /// <summary>
    /// Gets or sets the role instance.
    /// </summary>
    public RoleEntity Role { get; set; }

    /// <summary>
    /// Gets or sets the permission instance.
    /// </summary>
    public PermissionEntity Permission { get; set; }
}
