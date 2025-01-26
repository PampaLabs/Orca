namespace Orca
{
    /// <summary>
    /// Represents the configuration for ASP.NET, including events and authentication schemes.
    /// </summary>
    public class OrcaWebHost
    {
        /// <summary>
        /// Gets or sets the events related to authorization.
        /// </summary>
        public OrcaEvents Events { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of authentication schemes to be used.
        /// </summary>
        public List<string> Schemes { get; set; } = [];
    }
}
