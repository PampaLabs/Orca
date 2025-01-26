namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a delegation response.
/// </summary>
public class DelegationResponse
{
    /// <summary>
    /// The unique identifier for the delegation.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The subject who is delegating the permissions.
    /// </summary>
    public SubjectResponse Who { get; set; }

    /// <summary>
    /// The subject who is receiving the delegated permissions.
    /// </summary>
    public SubjectResponse Whom { get; set; }

    /// <summary>
    /// The start date and time of the delegation (when the delegation becomes effective).
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    /// The end date and time of the delegation (when the delegation expires).
    /// </summary>
    public DateTime To { get; set; }

    /// <summary>
    /// A value indicating whether the delegation is currently enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;
}
