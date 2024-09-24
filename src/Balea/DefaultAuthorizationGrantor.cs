using System.Security.Claims;

namespace Balea
{
    public class DefaultAuthorizationGrantor : IAuthorizationGrantor
    {
        private readonly BaleaOptions _options;

        private readonly IRoleStore _roleStore;
        private readonly IPermissionStore _permissionStore;
        private readonly IDelegationStore _delegationStore;
        private readonly IPolicyStore _policyStore;

        public DefaultAuthorizationGrantor(
            BaleaOptions options,
            IRoleStore roleStore,
            IPermissionStore permissionStore,
            IDelegationStore delegationStore,
            IPolicyStore policyStore
            )
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _roleStore = roleStore ?? throw new ArgumentNullException(nameof(roleStore));
            _permissionStore = permissionStore ?? throw new ArgumentNullException(nameof(permissionStore));
            _delegationStore = delegationStore ?? throw new ArgumentNullException(nameof(delegationStore));
            _policyStore = policyStore ?? throw new ArgumentNullException(nameof(policyStore));
        }

        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var sourceRoleClaims = user.GetClaimValues(_options.ClaimTypeMap.RoleClaimType).ToArray();

            var subject = user.GetSubjectId(_options);
            var delegation = await _delegationStore.FindBySubjectAsync(subject, cancellationToken);

            if (delegation is not null)
            {
                subject = delegation.Who;
            }

            var userRoles = await _roleStore.SearchAsync(new() { Subjects = [subject] }, cancellationToken);
            var claimRoles = await _roleStore.SearchAsync(new() { Mappings = sourceRoleClaims }, cancellationToken);

            var roles = Enumerable.Empty<Role>()
                .Union(userRoles)
                .Union(claimRoles)
                .Distinct()
                .ToList();

            var roleNames = roles.Select(x => x.Name).ToList();

            var permissions = await _permissionStore.SearchAsync(new() { Roles = [..roleNames] }, cancellationToken);

            return new AuthorizationContext
            {
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