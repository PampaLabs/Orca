using Microsoft.Extensions.Logging;

namespace Orca.Diagnostics
{
    internal static class Log
    {
        public static void AuthorizationPolicyFound(this ILogger logger, string policyName)
        {
            _authorizationPolicyFound(logger, policyName, null);
        }

        public static void AuthorizationPolicyNotFound(this ILogger logger, string policyName)
        {
            _authorizationPolicyNotFound(logger, policyName, null);
        }

        public static void CreatingAuthorizationPolicy(this ILogger logger, string policyName)
        {
            _creatingAuthorizationPolicy(logger, policyName, null);
        }

        public static void CreatingAuthorizationPolicy(this ILogger logger, string policyName, IEnumerable<string> schemes)
        {
            _creatingAuthorizationPolicyForSchemes(logger, policyName, schemes, null);
        }

        public static void OrcaRolesFoundForUser(this ILogger logger, string user, IEnumerable<string> roles)
        {
            _orcaRolesFoundForUser(logger, user, roles, null);
        }

        public static void NoOrcaRolesForUser(this ILogger logger, string user)
        {
            _noOrcaRolesForUser(logger, user, null);
        }

        public static void ExecutingOrcaUnauthorizedFallback(this ILogger logger)
        {
            _executingOrcaUnauthorizedFallback(logger, null);
        }

        public static void NoOrcaRolesForUserAndNoUnauthorizedFallback(this ILogger logger)
        {
            _noOrcaRolesForUserAndNoUnauthorizedFallback(logger, null);
        }

        public static void PolicySucceed(this ILogger logger)
        {
            _policySucceed(logger, null);
        }

        public static void PolicyFailToForbid(this ILogger logger)
        {
            _policyFail(logger, "Forbid", null);
        }

        public static void PolicyFailToChallenge(this ILogger logger)
        {
            _policyFail(logger, "Challenge", null);
        }

        public static void PopulatePropertyBag(this ILogger logger, string propertyBag)
        {
            _populatePropertyBag(logger, propertyBag, null);
        }

        public static void PropertyBagCantBePopulated(this ILogger logger, string propertyBag)
        {
            _propertyBagCantBePopulated(logger, propertyBag, null);
        }

        public static void AbacAuthorizationHandlerThrow(this ILogger logger, Exception exception)
        {
            _abacAuthorizationHandlerThrow(logger, exception);
        }

        public static void AbacAuthorizationHandlerIsEvaluatingPolicy(this ILogger logger, string policyName, string content)
        {
            _abacAuthorizationHandlerIsEvaluatingPolicy(logger, policyName, content, null);
        }

        public static void AbacAuthorizationHandlerEvaluationSuccesss(this ILogger logger, string policyName)
        {
            _abacAuthorizationHandlerEvaluationSuccess(logger, policyName, null);
        }

        public static void AbacDiscoverPropertyBagParameter(this ILogger logger, string propertyName, string propertyType)
        {
            _abacDiscoverPropertyBagParameter(logger, propertyName, propertyType, null);
        }

        private static readonly Action<ILogger, string, Exception> _populatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Information,
            EventIds.PropertyBag,
            "Populating property bag {bag}.");

        private static readonly Action<ILogger, string, Exception> _propertyBagCantBePopulated = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.PropertyBagCantBePopulated,
            "Populating property bag {bag} from authorization filter context.");

        private static readonly Action<ILogger, string, Exception> _authorizationPolicyFound = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.AuthorizationPolicyFound,
            formatString: "Found stored authorization policy: {policyName}.");

        private static readonly Action<ILogger, string, Exception> _authorizationPolicyNotFound = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.AuthorizationPolicyNotFound,
            formatString: "Authorization policy {policyName} not found.");

        private static readonly Action<ILogger, string, Exception> _creatingAuthorizationPolicy = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.CreatingAuthorizationPolicy,
            formatString: "Creating authorization policy {policyName} for default scheme.");

        private static readonly Action<ILogger, string, IEnumerable<string>, Exception> _creatingAuthorizationPolicyForSchemes = LoggerMessage.Define<string, IEnumerable<string>>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.CreatingAuthorizationPolicy,
            formatString: "Creating authorization policy {policyName} for schemes {schemes}.");

        private static readonly Action<ILogger, string, IEnumerable<string>, Exception> _orcaRolesFoundForUser = LoggerMessage.Define<string, IEnumerable<string>>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.NoOrcaRolesForUser,
            formatString: "Orca roles found for user {user}: {roles}");

        private static readonly Action<ILogger, string, Exception> _noOrcaRolesForUser = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.NoOrcaRolesForUser,
            formatString: "No Orca roles found for user {user}.");

        private static readonly Action<ILogger, Exception> _executingOrcaUnauthorizedFallback = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: EventIds.ExecutingOrcaUnauthorizedFallback,
            formatString: "Executing Orca unauthorized fallback.");

        private static readonly Action<ILogger, Exception> _noOrcaRolesForUserAndNoUnauthorizedFallback = LoggerMessage.Define(
            logLevel: LogLevel.Information,
            eventId: EventIds.NoOrcaRolesForUserAndNoUnauthorizedFallback,
            formatString: "No Orca roles found for the current user and no unauthorized fallback defined. Authorization behavior may be unexpected.");

        private static readonly Action<ILogger, Exception> _policySucceed = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: EventIds.PolicySucceed,
            formatString: "Policy succeed");

        private static readonly Action<ILogger, string, Exception> _policyFail = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.PolicyFail,
            formatString: "Policy failed. {policyResult}");

        private static readonly Action<ILogger, Exception> _abacAuthorizationHandlerThrow = LoggerMessage.Define(
           logLevel: LogLevel.Error,
           eventId: EventIds.AbacAuthorizationHandlerThrow,
           formatString: "The Abac authorization handler throw!.");

        private static readonly Action<ILogger, string, string, Exception> _abacDiscoverPropertyBagParameter = LoggerMessage.Define<string, string>(
           logLevel: LogLevel.Debug,
           eventId: EventIds.AbacDiscoverPropertyBagParameter,
           formatString: "Parameter property bag discover a new property with name {propertyName} and type {propertyType}.");

        private static readonly Action<ILogger, string, string, Exception> _abacAuthorizationHandlerIsEvaluatingPolicy = LoggerMessage.Define<string, string>(
           logLevel: LogLevel.Debug,
           eventId: EventIds.AbacAuthorizationHandlerIsEvaluationPolicy,
           formatString: "The Abac authorization handler is evaluating the policy {policyName} with content {policyContent}.");

        private static readonly Action<ILogger, string, Exception> _abacAuthorizationHandlerEvaluationSuccess = LoggerMessage.Define<string>(
           logLevel: LogLevel.Debug,
           eventId: EventIds.AbacAuthorizationHandlerEvaluationSuccess,
           formatString: "The Abac authorization handler evaluate success the policy {policyName}.");
    }
}
