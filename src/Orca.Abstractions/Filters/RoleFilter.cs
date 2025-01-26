namespace Orca
{
    /// <summary>
    /// Represents a filter for querying roles.
    /// </summary>
    public class RoleFilter
    {
        /// <summary>
        /// Gets or sets the name to filter roles by.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description to filter roles by.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the enabled status to filter roles by.
        /// Can be null to indicate no filter on the enabled status.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets an array of mappings to filter roles by.
        /// Mappings could represent resources or permissions associated with the role.
        /// </summary>
        public string[] Mappings { get; set; }
    }
}
