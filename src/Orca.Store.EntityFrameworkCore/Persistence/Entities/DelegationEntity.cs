namespace Orca.Store.EntityFrameworkCore.Entities;

/// <summary>
/// Represents a delegation entity.
/// </summary>
public class DelegationEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the delegation.
    /// </summary>
    public string Id { get; set; }


    /// <summary>
    /// Gets or sets the subject ID who is delegating the permissions.
    /// </summary>
    public string WhoId { get; set; }

    /// <summary>
    /// Gets or sets the subject ID who is receiving the delegated permissions.
    /// </summary>
    public string WhomId { get; set; }

    /// <summary>
    /// Gets or sets the start date and time of the delegation (when the delegation becomes effective).
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    /// Gets or sets the end date and time of the delegation (when the delegation expires).
    /// </summary>
    public DateTime To { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the delegation is currently enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;


    /// <summary>
    /// Gets or sets the subject instance who is delegating the permissions.
    /// </summary>
    public SubjectEntity Who { get; set; }

    /// <summary>
    /// Gets or sets the subject instance who is receiving the delegated permissions.
    /// </summary>
    public SubjectEntity Whom { get; set; }
}
