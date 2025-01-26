using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Defines methods for authorizing a user and retrieving policies.
    /// </summary>
    public interface IAuthorizationGrantor
    {
        /// <summary>
        /// Finds the authorization context for a given user.
        /// </summary>
        /// <param name="user">The user whose authorization context is being requested.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AuthorizationContext"/>.</returns>
        Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a policy by its name.
        /// </summary>
        /// <param name="name">The name of the policy to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the requested <see cref="Policy"/>.</returns>
        Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default);
    }
}
