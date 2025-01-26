using Microsoft.AspNetCore.Authorization;

namespace Orca.Authorization.Abac
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified authorization using Orca DSL.
    /// </summary>
    public class AbacAuthorizeAttribute
        : AuthorizeAttribute
    {
        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        /// <param name="policy">The ABAC policy  registered on Orca to be used.</param>
        public AbacAuthorizeAttribute(string policy) :
            base(new AbacPrefix(policy).ToString())
        { }
    }
}
