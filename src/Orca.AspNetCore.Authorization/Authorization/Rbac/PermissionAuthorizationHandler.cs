using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Orca.Authorization.Rbac
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly OrcaOptions _options;

        public PermissionAuthorizationHandler(IOptions<OrcaOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var succeed = false;

            if (context.User.Identity.IsAuthenticated)
            {
                var permissionEvaluator = new PermissionEvaluator(_options.ClaimTypeMap);

                if (permissionEvaluator.HasPermission(context.User, requirement.Name))
                {
                    succeed = true;
                }
            }

            if (succeed)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
