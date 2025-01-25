using Orca;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
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

        public static IEnumerable<string> GetClaimValues(
            this ClaimsPrincipal principal,
            string claimType)
        {
            return principal.FindAll(claimType)
                .Select(x => x.Value);
        }
    }
}
