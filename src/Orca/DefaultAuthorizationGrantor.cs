using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Orca
{
    /// <inheritdoc />
    public class DefaultAuthorizationGrantor : IAuthorizationGrantor
    {
        private readonly OrcaOptions _options;

        private readonly IOrcaStoreAccessor _storeAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthorizationGrantor"/> class.
        /// </summary>
        /// <param name="options">The authorizations options.</param>
        /// <param name="storeAccessor">The store accessor.</param>
        public DefaultAuthorizationGrantor(IOptions<OrcaOptions> options, IOrcaStoreAccessor storeAccessor)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _storeAccessor = storeAccessor ?? throw new ArgumentNullException(nameof(storeAccessor));
        }

        /// <inheritdoc />
        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var sourceRoleClaims = principal.GetClaimValues(_options.ClaimTypeMap.RoleClaimType).ToArray();

            var subject = principal.GetSubjectId(_options);

            var user = await _storeAccessor.SubjectStore.FindBySubAsync(subject, cancellationToken);

            if (user is null)
            {
                return new AuthorizationContext();
            }

            var delegation = await _storeAccessor.DelegationStore.FindBySubjectAsync(subject, cancellationToken);

            var represent = delegation?.Who ?? user;

            var userRoles = await _storeAccessor.SubjectStore.GetRolesAsync(represent, cancellationToken);
            var claimRoles = await _storeAccessor.RoleStore.SearchAsync(new() { Mappings = sourceRoleClaims }, cancellationToken);

            var roles = Enumerable.Empty<Role>()
                .Union(userRoles)
                .Union(claimRoles)
                .DistinctBy(role => role.Id)
                .ToList();

            var roleNames = roles.Select(x => x.Name).ToList();

            var permissions = await _storeAccessor.PermissionStore.SearchAsync(new() { Roles = [.. roleNames] }, cancellationToken);

            return new AuthorizationContext
            {
                Subject = user,
                Roles = roles,
                Permissions = permissions,
                Delegation = delegation,
            };
        }

        /// <inheritdoc />
        public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _storeAccessor.PolicyStore.FindByNameAsync(name, cancellationToken);
        }
    }
}