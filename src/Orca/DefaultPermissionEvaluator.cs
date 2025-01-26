using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Orca
{
    /// <summary>
    /// Check if OrcaMiddleware authentication scheme has claims permissions fot the current user. 
    /// </summary>
    public class DefaultPermissionEvaluator : IPermissionEvaluator
    {
        private readonly OrcaOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPermissionEvaluator"/> class.
        /// </summary>
        /// <param name="options">The Orca options that configure the claim types used for permission evaluation.</param>
        public DefaultPermissionEvaluator(IOptions<OrcaOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Asynchronously checks if the specified user has the specified permission.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the user has the specified permission; otherwise, false.</returns>
        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var hasPermission = user
                .GetClaimValues(_options.ClaimTypeMap.PermissionClaimType)
                .Any(claimValue => claimValue.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(hasPermission);
        }
    }
}
