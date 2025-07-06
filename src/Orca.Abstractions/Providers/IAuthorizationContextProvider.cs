using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Defines a service that provides an <see cref="AuthorizationContext"/> for a given user.
    /// </summary>
    public interface IAuthorizationContextProvider
    {
        /// <summary>
        /// Creates the authorization context for a given user.
        /// </summary>
        /// <param name="user">The user whose authorization context is being requested.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AuthorizationContext"/>.</returns>
        Task<AuthorizationContext> CreateAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }
}
