namespace Orca
{
    /// <summary>
    /// Represents the options to configuring the authorization.
    /// </summary>
    public class OrcaAuthorizationOptions
    {
        /// <summary>
        /// Gets or sets the events related to authorization.
        /// </summary>
        public OrcaAuthorizationEvents Events { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of authentication schemes to be used.
        /// </summary>
        public List<string> Schemes { get; set; } = [];
    }
}
