namespace Orca
{
    /// <summary>
    /// Represents a filter for querying policies.
    /// </summary>
    public class PolicyFilter
    {
        /// <summary>
        /// Gets or sets the name to filter policies by.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description to filter policies by.
        /// </summary>
        public string Description { get; set; }
    }
}
