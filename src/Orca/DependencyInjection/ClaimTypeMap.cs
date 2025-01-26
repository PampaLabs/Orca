using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Represents the mapping of claim types.
    /// </summary>
    public class ClaimTypeMap
    {
        /// <summary>
        /// Gets or sets the claim type used for roles.
        /// Defaults to <see cref="ClaimTypes.Role"/>.
        /// </summary>
        public string RoleClaimType { get; set; } = ClaimTypes.Role;

        /// <summary>
        /// Gets or sets the claim type used for names.
        /// Defaults to <see cref="ClaimTypes.Name"/>.
        /// </summary>
        public string NameClaimType { get; set; } = ClaimTypes.Name;

        /// <summary>
        /// Gets the collection of allowed subject claim types. 
        /// Defaults to include <see cref="ClaimTypes.NameIdentifier"/> and <see cref="JwtClaimTypes.ClientId"/>.
        /// </summary>
        public ICollection<string> AllowedSubjectClaimTypes { get; } =
        [
            ClaimTypes.NameIdentifier,
            JwtClaimTypes.ClientId
        ];

        /// <summary>
        /// Gets or sets the claim type used for permissions.
        /// Defaults to <see cref="OrcaClaims.Permission"/>.
        /// </summary>
        public string PermissionClaimType { get; set; } = OrcaClaims.Permission;
    }
}
