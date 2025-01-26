namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a permission entity.
/// </summary>
public class PermissionEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the permission, explaining what it allows or represents.
    /// </summary>
    public string Description { get; set; }
}
