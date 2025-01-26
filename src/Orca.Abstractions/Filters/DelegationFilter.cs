namespace Orca
{
    /// <summary>
    /// Represents a filter for querying delegations.
    /// </summary>
    public class DelegationFilter
    {
        /// <summary>
        /// Gets or sets the identifier or name of the subject who is delegating the permissions.
        /// </summary>
        public string Who { get; set; }

        /// <summary>
        /// Gets or sets the identifier or name of the subject who is receiving the delegated permissions.
        /// </summary>
        public string Whom { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the delegation to filter delegations by.
        /// Only delegations starting on or after this date will be included.
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the delegation to filter delegations by.
        /// Only delegations ending on or before this date will be included.
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the delegation is enabled.
        /// Can be null to indicate no filter on the enabled status.
        /// </summary>
        public bool? Enabled { get; set; }
    }
}
