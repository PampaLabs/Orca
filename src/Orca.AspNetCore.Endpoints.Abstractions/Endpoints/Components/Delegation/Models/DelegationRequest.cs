namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a delegation request.
/// </summary>
public class DelegationRequest
{

    /// <summary>
    /// The subject who is delegating the permissions.
    /// </summary>
    public string Who { get; set; }

    /// <summary>
    /// The subject who is receiving the delegated permissions.
    /// </summary>
    public string Whom { get; set; }

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
