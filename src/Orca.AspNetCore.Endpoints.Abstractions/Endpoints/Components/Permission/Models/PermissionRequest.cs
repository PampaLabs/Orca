namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a permission request.
/// </summary>
public class PermissionRequest
{
    /// <summary>
    /// The name of the permission.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the permission, explaining what it allows or represents.
    /// </summary>
    public string Description { get; set; }
}
