using Orca;

namespace System.Security.Claims
{
    /// <summary>
    /// Extension methods for <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the subject identifier.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <param name="options">The authorizations options.</param>
        /// <returns></returns>
        public static string GetSubjectId(this ClaimsPrincipal principal, OrcaOptions options)
        {
            string sid = null;

            foreach(var allowedSubjectClaimType in options.ClaimTypeMap.AllowedSubjectClaimTypes)
            {
                sid = principal.FindFirstValue(allowedSubjectClaimType);

                if ( sid != null )
                {
                    break;
                }
            }

            _ = sid ?? throw new InvalidOperationException($"Orca allowed subject claim type is missing.");
        
            return sid;
        }

        /// <summary>
        /// Gets the claim values.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <param name="claimType">The claims type.</param>
        /// <returns>The claim values</returns>
        public static IEnumerable<string> GetClaimValues(
            this ClaimsPrincipal principal,
            string claimType)
        {
            return principal.FindAll(claimType)
                .Select(x => x.Value);
        }
    }
}
