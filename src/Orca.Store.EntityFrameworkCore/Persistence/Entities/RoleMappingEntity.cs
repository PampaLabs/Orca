namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a role mapping entity.
/// </summary>
public class RoleMappingEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the mapping.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the mapping.
    /// </summary>
    public string Mapping { get; set; }

    /// <summary>
    /// Gets or sets the role ID of the mapping.
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role instance of the mapping.
    /// </summary>
    public RoleEntity Role { get; set; }
}
