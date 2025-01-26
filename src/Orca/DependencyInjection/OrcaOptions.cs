namespace Orca
{
    /// <summary>
    /// Represents the options for configuring authorization.
    /// </summary>
    public class OrcaOptions
    {
        /// <summary>
        /// Gets or sets the claim type map.
        /// </summary>
        public ClaimTypeMap ClaimTypeMap { get; set; } = new ClaimTypeMap();
    }
}
