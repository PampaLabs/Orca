namespace Orca
{
    /// <summary>
    /// Represents the context of authorization.
    /// </summary>
    public class AuthorizationContext
    {
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public Subject Subject { get; set; }

        /// <summary>
        /// Gets or sets the collection of roles assigned.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of permissions granted.
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets the delegation associated with the subject.
        /// </summary>
        public Delegation Delegation { get; set; }
    }
}
