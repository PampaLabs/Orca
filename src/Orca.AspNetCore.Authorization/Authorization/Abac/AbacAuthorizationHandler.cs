using Orca.Authorization.Abac.Context;
using Orca.Authorization.Abac.Grammars;
using Orca.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Orca.Authorization.Abac
{
    internal class AbacAuthorizationHandler : AuthorizationHandler<AbacRequirement>
    {
        private readonly AbacAuthorizationContextFactory _abacAuthorizationContextFactory;
        private readonly IAuthorizationGrantor _runtimeAuthorizationServerStore;
        private readonly ILogger<AbacAuthorizationHandler> _logger;

        public AbacAuthorizationHandler(
            AbacAuthorizationContextFactory abacAuthorizationContextFactory,
            IAuthorizationGrantor runtimeAuthorizationServerStore,
            ILogger<AbacAuthorizationHandler> logger)
        {
            Ensure.Argument.NotNull(abacAuthorizationContextFactory, nameof(abacAuthorizationContextFactory));
            Ensure.Argument.NotNull(runtimeAuthorizationServerStore, nameof(runtimeAuthorizationServerStore));
            Ensure.Argument.NotNull(logger, nameof(logger));
            _abacAuthorizationContextFactory = abacAuthorizationContextFactory;
            _runtimeAuthorizationServerStore = runtimeAuthorizationServerStore;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                try
                {
                    var policy = await _runtimeAuthorizationServerStore
                        .GetPolicyAsync(requirement.Name);
                
                    if (policy is object)
                    {
                        Log.AbacAuthorizationHandlerIsEvaluatingPolicy(_logger, policy.Name, policy.Content);

                        var abacContext = await _abacAuthorizationContextFactory.Create(context);
                        var abacPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy.Content, WellKnownGrammars.Bal);
                    
                        if (abacPolicy.IsSatisfied(abacContext))
                        {
                            Log.AbacAuthorizationHandlerEvaluationSuccesss(_logger,policy.Name);
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.AbacAuthorizationHandlerThrow(_logger, ex);
                }
            }

            context.Fail();
        }
    }
}
