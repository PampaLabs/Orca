using System.Security.Claims;

namespace Balea
{
    public interface IAuthorizationGrantor
    {
        Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default);
    }
}
