using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Provides functionality to evaluate whether a user has specific permissions.
    /// </summary>
    public class PermissionEvaluator
    {
        private readonly ClaimTypeMap _claimTypeMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionEvaluator"/> class.
        /// </summary>
        /// <param name="claimTypeMap">The claim mapping options.</param>
        public PermissionEvaluator(ClaimTypeMap claimTypeMap)
        {
            _claimTypeMap = claimTypeMap ?? throw new ArgumentNullException(nameof(claimTypeMap));
        }

        /// <summary>
        /// Checks if the specified user has the specified permission.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>A <see cref="bool"/> value indicating whether the user has the permission.</returns>
        public bool HasPermission(ClaimsPrincipal user, string permission)
        {
            return user
                .GetClaimValues(_claimTypeMap.PermissionClaimType)
                .Any(claimValue => claimValue.Equals(permission, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
