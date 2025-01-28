using Microsoft.AspNetCore.Http;

namespace Orca
{
    /// <summary>
    /// Represents events and delegates associated with authorization.
    /// </summary>
    public class OrcaAuthorizationEvents
    {
        /// <summary>
        /// Gets or sets the fallback request delegate to handle unauthorized access.
        /// </summary>
        public RequestDelegate UnauthorizedFallback { get; set; }
    }
}
