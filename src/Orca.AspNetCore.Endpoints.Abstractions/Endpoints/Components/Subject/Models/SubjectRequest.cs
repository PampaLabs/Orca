namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a subject request.
/// </summary>
public class SubjectRequest
{
    /// <summary>
    /// The value associated with the subject.
    /// </summary>
    public string Sub { get; set; }

    /// <summary>
    /// The name of the subject.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The email address associated with the subject.
    /// </summary>
    public string Email { get; set; }
}
