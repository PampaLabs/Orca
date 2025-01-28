using Orca.Authorization.Abac;
using Orca.Authorization.Rbac;
using Orca.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Orca.Authorization
{
    /// <summary>
    /// A custom implementation of the <see cref="DefaultAuthorizationPolicyProvider"/> to retrieve and create authorization policies.
    /// </summary>
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _aspOptions;
        private readonly OrcaAuthorizationOptions _orcaOptions;
        private readonly ILogger<AuthorizationPolicyProvider> _logger;

        private object sync_root = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="aspOptions">The AspNet authorization options.</param>
        /// <param name="orcaOptions">The Orca authorization options.</param>
        /// <param name="logger">The logger used to log events and errors in policy evaluation.</param>
        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> aspOptions,
            IOptions<OrcaAuthorizationOptions> orcaOptions,
            ILogger<AuthorizationPolicyProvider> logger)
            : base(aspOptions)
        {
            _aspOptions = aspOptions.Value;
            _orcaOptions = orcaOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                _logger.AuthorizationPolicyNotFound(policyName);

                //setup abac or rbac requirement
                var abacPrefix = new AbacPrefix(policyName);
                var requirement = policyName.Equals(abacPrefix.ToString())
                    ? (IAuthorizationRequirement) new AbacRequirement(abacPrefix.Policy)
                    :  new PermissionRequirement(policyName);

                if (_orcaOptions.Schemes.Any())
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .AddAuthenticationSchemes(_orcaOptions.Schemes.ToArray())
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName, _orcaOptions.Schemes);
                }
                else
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName);
                }

                lock (sync_root)
                {
                    // By default, policies are stored in the AuthorizationOptions instance (singleton),
                    // so we can cache all the policies created at runtime there to create the policies only once
                    // the internal dictionary is a plain dictionary ( not concurrent ), we need to ensure
                    // a thread safe access 
                    _aspOptions.AddPolicy(policyName, policy);
                }
            }
            else
            {
                _logger.AuthorizationPolicyFound(policyName);
            }

            return policy;
        }
    }
}
