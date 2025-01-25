using System.Security.Claims;
using Orca.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Orca.Authorization
{
    public class OrcaPolicyEvaluator : IPolicyEvaluator
    {
        private readonly IAuthorizationService _authorization;
        private readonly IAuthorizationGrantor _store;
        private readonly OrcaOptions _options;
        private readonly OrcaWebHost _webHost;
        private readonly ILogger<OrcaPolicyEvaluator> _logger;

        public OrcaPolicyEvaluator(
            IAuthorizationService authorization,
            IAuthorizationGrantor store,
            IOptions<OrcaOptions> options,
            IOptions<OrcaWebHost> webHost,
            ILogger<OrcaPolicyEvaluator> logger)
        {
            _authorization = authorization;
            _store = store;
            _options = options.Value;
            _webHost = webHost.Value;
            _logger = logger;
        }

        public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var hasSchemes = _webHost.Schemes.Any();

            if (policy.AuthenticationSchemes != null && policy.AuthenticationSchemes.Count > 0)
            {
                ClaimsPrincipal newPrincipal = null;
                var matchPolicySchemes = false;
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    if (_webHost.Schemes.Any(s => s.Equals(scheme, StringComparison.OrdinalIgnoreCase)))
                    {
                        matchPolicySchemes = true;
                    }

                    var result = await context.AuthenticateAsync(scheme);
                    if (result != null && result.Succeeded)
                    {
                        newPrincipal = SecurityHelper.MergeUserPrincipal(newPrincipal, result.Principal);
                    }
                }

                if (newPrincipal != null)
                {
                    context.User = newPrincipal;

                    if (matchPolicySchemes)
                    {
                        await AddOrcaIdentity(context.User, context);
                    }

                    return AuthenticateResult.Success(new AuthenticationTicket(newPrincipal, string.Join(";", policy.AuthenticationSchemes)));
                }

                context.User = new ClaimsPrincipal(new ClaimsIdentity());
                return AuthenticateResult.NoResult();
            }

            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                // Only apply policies if it is configured without specific schemes
                if (!hasSchemes)
                {
                    await AddOrcaIdentity(context.User, context);
                }

                return AuthenticateResult.Success(new AuthenticationTicket(context.User, "context.User"));
            }

            return AuthenticateResult.NoResult();
        }

        public async Task<PolicyAuthorizationResult> AuthorizeAsync(
            AuthorizationPolicy policy,
            AuthenticateResult authenticationResult,
            HttpContext context,
            object resource)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            var result = await _authorization.AuthorizeAsync(context.User, resource, policy);

            if (result.Succeeded)
            {
                _logger.PolicySucceed();
                return PolicyAuthorizationResult.Success();
            }

            // If authentication was successful, return forbidden, otherwise challenge
            if (authenticationResult.Succeeded)
            {
                _logger.PolicyFailToForbid();
                return PolicyAuthorizationResult.Forbid();
            }

            _logger.PolicyFailToChallenge();
            return PolicyAuthorizationResult.Challenge();
        }

        private async Task AddOrcaIdentity(ClaimsPrincipal user, HttpContext context)
        {
            var authorization = await _store
                .FindAuthorizationAsync(user);

            if (authorization.Roles.Any())
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.OrcaRolesFoundForUser(user.GetSubjectId(_options), authorization.Roles.Select(r => r.Name));
                }
            }
            else
            {
                // A user without mapping roles is an unauthorized user, because we can not match roles or permissions.
                // If the user has not roles, we try to execute the unauthorized fallback to be consistent with this principle.
                // If there is not an unauthorized fallback defined, the authorizations result may be unexpected.
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.NoOrcaRolesForUser(user.GetSubjectId(_options));
                }

                if (!context.Response.HasStarted && _webHost.Events.UnauthorizedFallback != null)
                {
                    _logger.ExecutingOrcaUnauthorizedFallback();

                    await _webHost.Events.UnauthorizedFallback(context);
                }
                else
                {
                    _logger.NoOrcaRolesForUserAndNoUnauthorizedFallback();
                }

                return;
            }

            var roleClaims = authorization.Roles
                .Where(role => role.Enabled)
                .Select(role => new Claim(_options.ClaimTypeMap.RoleClaimType, role.Name));

            var permissionClaims = authorization.Permissions
                .Select(permission => new Claim(_options.ClaimTypeMap.PermissionClaimType, permission.Name));

            var identity = new ClaimsIdentity(
                authenticationType: "Orca",
                nameType: _options.ClaimTypeMap.NameClaimType,
                roleType: _options.ClaimTypeMap.RoleClaimType);

            identity.AddClaims(roleClaims);
            identity.AddClaims(permissionClaims);

            if (authorization.Delegation != null)
            {
                identity.AddClaim(new Claim(OrcaClaims.DelegatedBy, authorization.Delegation.Who.Sub));
                identity.AddClaim(new Claim(OrcaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
                identity.AddClaim(new Claim(OrcaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
            }

            user.AddIdentity(identity);
        }
    }

    internal static class SecurityHelper
    {
        /// <summary>
        /// Add all ClaimsIdentities from an additional ClaimPrincipal to the ClaimsPrincipal
        /// Merges a new claims principal, placing all new identities first, and eliminating
        /// any empty unauthenticated identities from context.User
        /// </summary>
        /// <param name="existingPrincipal">The <see cref="ClaimsPrincipal"/> containing existing <see cref="ClaimsIdentity"/>.</param>
        /// <param name="additionalPrincipal">The <see cref="ClaimsPrincipal"/> containing <see cref="ClaimsIdentity"/> to be added.</param>
        public static ClaimsPrincipal MergeUserPrincipal(ClaimsPrincipal existingPrincipal, ClaimsPrincipal additionalPrincipal)
        {
            var newPrincipal = new ClaimsPrincipal();

            // New principal identities go first
            if (additionalPrincipal != null)
            {
                newPrincipal.AddIdentities(additionalPrincipal.Identities);
            }

            // Then add any existing non empty or authenticated identities
            if (existingPrincipal != null)
            {
                newPrincipal.AddIdentities(existingPrincipal.Identities.Where(i => i.IsAuthenticated || i.Claims.Any()));
            }
            return newPrincipal;
        }
    }
}