using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Provides functionality to create a <see cref="ClaimsIdentity"/> for a user.
    /// </summary>
    public class ClaimsIdentityFactory
    {
        private readonly ClaimTypeMap _claimTypeMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsIdentityFactory"/> class.
        /// </summary>
        /// <param name="claimTypeMap">The claim type mapping configuration to use when building the identity.</param>
        public ClaimsIdentityFactory(ClaimTypeMap claimTypeMap)
        {
            _claimTypeMap = claimTypeMap ?? throw new ArgumentNullException(nameof(claimTypeMap));
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> populated with claims based on the specified authorization context.
        /// </summary>
        /// <param name="context">The <see cref="AuthorizationContext"/> representing the user's roles, permissions, and delegations.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the user's claims.</returns>
        public ClaimsIdentity Create(AuthorizationContext context)
        {
            var identity = CreateClaimsIdentity();

            AddRoleClaims(identity, context);
            AddPermissionClaims(identity, context);
            AddDelegationClaims(identity, context);

            return identity;
        }

        private ClaimsIdentity CreateClaimsIdentity()
        {
            return new ClaimsIdentity(
                authenticationType: "Orca",
                nameType: _claimTypeMap.NameClaimType,
                roleType: _claimTypeMap.RoleClaimType);
        }

        private void AddRoleClaims(ClaimsIdentity identity, AuthorizationContext authorization)
        {
            var roleClaims = authorization.Roles
                .Where(role => role.Enabled)
                .Select(role => new Claim(_claimTypeMap.RoleClaimType, role.Name));

            identity.AddClaims(roleClaims);
        }

        private void AddPermissionClaims(ClaimsIdentity identity, AuthorizationContext authorization)
        {
            var permissionClaims = authorization.Permissions
                .Select(permission => new Claim(_claimTypeMap.PermissionClaimType, permission.Name));

            identity.AddClaims(permissionClaims);
        }

        private void AddDelegationClaims(ClaimsIdentity identity, AuthorizationContext authorization)
        {
            if (authorization.Delegation == null) return;

            identity.AddClaim(new Claim(OrcaClaims.DelegatedBy, authorization.Delegation.Who.Sub));
            identity.AddClaim(new Claim(OrcaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
            identity.AddClaim(new Claim(OrcaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
        }
    }
}