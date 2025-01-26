namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a role subject entity.
/// </summary>
public class RoleSubjectEntity
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// Gets or sets the subject ID.
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the role instance.
    /// </summary>
    public RoleEntity Role { get; set; }

    /// <summary>
    /// Gets or sets the subject instance.
    /// </summary>
    public SubjectEntity Subject { get; set; }
}
