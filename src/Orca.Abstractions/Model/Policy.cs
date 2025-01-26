namespace Orca
{
    /// <summary>
    /// Represents a policy.
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// Gets or sets the unique identifier for the policy.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the policy, explaining its purpose and scope.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content of the policy, which may include the rules, conditions, or logic the policy enforces.
        /// </summary>
        public string Content { get; set; }
    }
}
