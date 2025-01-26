namespace Orca
{
    /// <summary>
    /// Represents a delegation.
    /// </summary>
    public class Delegation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the delegation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subject who is delegating the permissions.
        /// </summary>
        public Subject Who { get; set; }

        /// <summary>
        /// Gets or sets the subject who is receiving the delegated permissions.
        /// </summary>
        public Subject Whom { get; set; }

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
        /// Gets a value indicating whether the delegation is currently active based on the enabled status and time window.
        /// </summary>
        public bool Active => Enabled && From <= DateTime.UtcNow && To >= DateTime.UtcNow;
    }
}
