namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a role entity.
/// </summary>
public class RoleEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the role is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the collection of mappings associated with the role.
    /// </summary>
    public List<RoleMappingEntity> Mappings { get; set; } = [];
}
