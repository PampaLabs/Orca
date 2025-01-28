namespace Orca
{
    /// <summary>
    /// Represents the options to configuring the core capability.
    /// </summary>
    public class OrcaOptions
    {
        /// <summary>
        /// Gets or sets the claim type map.
        /// </summary>
        public ClaimTypeMap ClaimTypeMap { get; set; } = new ClaimTypeMap();
    }
}
