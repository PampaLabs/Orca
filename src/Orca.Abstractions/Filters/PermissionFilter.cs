namespace Orca
{
    /// <summary>
    /// Represents a filter for querying permissions.
    /// </summary>
    public class PermissionFilter
    {
        /// <summary>
        /// Gets or sets the name to filter permissions by.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description to filter permissions by.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets an array of roles to filter permissions by.
        /// Permissions associated with these roles will be included in the results.
        /// </summary>
        public string[] Roles { get; set; }
    }
}
