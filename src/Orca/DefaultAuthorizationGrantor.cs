using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Orca
{
    public class DefaultAuthorizationGrantor : IAuthorizationGrantor
    {
        private readonly OrcaOptions _options;

        private readonly ISubjectStore _subjectStore;
        private readonly IRoleStore _roleStore;
        private readonly IPermissionStore _permissionStore;
        private readonly IDelegationStore _delegationStore;
        private readonly IPolicyStore _policyStore;

        public DefaultAuthorizationGrantor(
            IOptions<OrcaOptions> options,
            ISubjectStore subjectStore,
            IRoleStore roleStore,
            IPermissionStore permissionStore,
            IDelegationStore delegationStore,
            IPolicyStore policyStore
            )
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));

            _subjectStore = subjectStore ?? throw new ArgumentNullException(nameof(subjectStore));
            _roleStore = roleStore ?? throw new ArgumentNullException(nameof(roleStore));
            _permissionStore = permissionStore ?? throw new ArgumentNullException(nameof(permissionStore));
            _delegationStore = delegationStore ?? throw new ArgumentNullException(nameof(delegationStore));
            _policyStore = policyStore ?? throw new ArgumentNullException(nameof(policyStore));
        }

        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var sourceRoleClaims = principal.GetClaimValues(_options.ClaimTypeMap.RoleClaimType).ToArray();

            var subject = principal.GetSubjectId(_options);

            var user = await _subjectStore.FindBySubAsync(subject, cancellationToken);

            if (user is null)
            {
                return new AuthorizationContext();
            }

            var delegation = await _delegationStore.FindBySubjectAsync(subject, cancellationToken);

            var represent = delegation?.Who ?? user;

            var userRoles = await _subjectStore.GetRolesAsync(represent, cancellationToken);
            var claimRoles = await _roleStore.SearchAsync(new() { Mappings = sourceRoleClaims }, cancellationToken);

            var roles = Enumerable.Empty<Role>()
                .Union(userRoles)
                .Union(claimRoles)
                .DistinctBy(role => role.Id)
                .ToList();

            var roleNames = roles.Select(x => x.Name).ToList();

            var permissions = await _permissionStore.SearchAsync(new() { Roles = [.. roleNames] }, cancellationToken);

            return new AuthorizationContext
            {
                Subject = user,
                Roles = roles,
                Permissions = permissions,
                Delegation = delegation,
            };
        }

        public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _policyStore.FindByNameAsync(name, cancellationToken);
        }
    }
}