namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a role response.
/// </summary>
public class RoleResponse
{
    /// <summary>
    ///The unique identifier for the role.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the role.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// A value indicating whether the role is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The collection of mappings associated with the role.
    /// </summary>
    public IList<string> Mappings { get; set; }
}
