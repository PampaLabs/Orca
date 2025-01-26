namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a permission response.
/// </summary>
public class PermissionResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the permission.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the permission, explaining what it allows or represents.
    /// </summary>
    public string Description { get; set; }
}
