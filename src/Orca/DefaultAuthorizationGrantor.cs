using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Orca
{
    /// <inheritdoc />
    public class DefaultAuthorizationGrantor : IAuthorizationGrantor
    {
        private readonly OrcaOptions _options;

        private readonly IAccessControlContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthorizationGrantor"/> class.
        /// </summary>
        /// <param name="options">The options used for configuring the authorization process.</param>
        /// <param name="context">The access control context that provides the stores.</param>
        public DefaultAuthorizationGrantor(IOptions<OrcaOptions> options, IAccessControlContext context)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var sourceRoleClaims = principal.GetClaimValues(_options.ClaimTypeMap.RoleClaimType).ToArray();

            var subject = principal.GetSubjectId(_options);

            var user = await _context.SubjectStore.FindBySubAsync(subject, cancellationToken);

            if (user is null)
            {
                return new AuthorizationContext();
            }

            var delegation = await _context.DelegationStore.FindBySubjectAsync(subject, cancellationToken);

            var represent = delegation?.Who ?? user;

            var userRoles = await _context.SubjectStore.GetRolesAsync(represent, cancellationToken);
            var claimRoles = await _context.RoleStore.SearchAsync(new() { Mappings = sourceRoleClaims }, cancellationToken);

            var roles = Enumerable.Empty<Role>()
                .Union(userRoles)
                .Union(claimRoles)
                .DistinctBy(role => role.Id)
                .ToList();

            var roleNames = roles.Select(x => x.Name).ToList();

            var permissions = await _context.PermissionStore.SearchAsync(new() { Roles = [.. roleNames] }, cancellationToken);

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
            return await _context.PolicyStore.FindByNameAsync(name, cancellationToken);
        }
    }
}