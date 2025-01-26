using Orca.Authorization.Abac.Grammars;
using Orca.Authorization.Abac.Parsers;

namespace Orca.Authorization.Abac.Context
{
    /// <summary>
    /// Represents an Attribute-Based Access Control (ABAC) authorization policy that contains a set of rules.
    /// This policy can be evaluated against an <see cref="AbacAuthorizationContext"/> to determine if the policy is satisfied.
    /// </summary>
    public class AbacAuthorizationPolicy
    {
        private readonly List<AbacAuthorizationRule> _authorizationRules = new List<AbacAuthorizationRule>();

        /// <summary>
        /// Gets the name of the policy.
        /// </summary>
        public string PolicyName { get; private set; }

        internal AbacAuthorizationPolicy(string policyName)
        {
            PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

        /// <summary>
        /// Evaluates the policy to determine if it is satisfied based on the provided <see cref="AbacAuthorizationContext"/>.
        /// </summary>
        /// <param name="abacAuthorizationContext">The context containing the attributes for evaluation.</param>
        /// <returns>True if the policy is satisfied; otherwise, false.</returns>
        public bool IsSatisfied(AbacAuthorizationContext abacAuthorizationContext)
        {
            if (abacAuthorizationContext == null)
            {
                throw new ArgumentNullException(nameof(abacAuthorizationContext));
            }

            var isSatisfied = true;

            foreach (var rule in _authorizationRules)
            {
                //evaluate all rules in the policy, checking if is a deny rule
                isSatisfied = isSatisfied && !(rule.Evaluate(abacAuthorizationContext) ^ !rule.IsDenyRule);
            }

            return isSatisfied;
        }

        internal void AddRule(AbacAuthorizationRule rule)
        {
            Ensure.NotNull(rule);

            _authorizationRules.Add(rule);
        }

        /// <summary>
        /// Creates an <see cref="AbacAuthorizationPolicy"/> from the specified policy string and grammar.
        /// </summary>
        /// <param name="policy">The policy string to be parsed.</param>
        /// <param name="grammar">The grammar to use for parsing the policy. Defaults to <see cref="WellKnownGrammars.Bal"/>.</param>
        /// <returns>An <see cref="AbacAuthorizationPolicy"/> created from the policy string.</returns>
        public static AbacAuthorizationPolicy CreateFromGrammar(string policy, WellKnownGrammars grammar = WellKnownGrammars.Bal)
        {
            try
            {
                return DefaultParser.Parse(policy, grammar);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Policy can't be parsed using the  grammar {Enum.GetName(typeof(WellKnownGrammars), grammar)} and policy is not created succcesfully.", exception);
            }
        }
    }
}
