namespace Orca.AspNetCore.Endpoints;

/// <summary>
/// Represents a policy request.
/// </summary>
public class PolicyRequest
{
    /// <summary>
    /// The name of the policy.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tthe description of the policy, explaining its purpose and scope.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The content of the policy, which may include the rules, conditions, or logic the policy enforces.
    /// </summary>
    public string Content { get; set; }
}
