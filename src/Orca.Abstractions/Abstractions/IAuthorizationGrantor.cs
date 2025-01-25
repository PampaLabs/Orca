using System.Security.Claims;

namespace Orca
{
    public interface IAuthorizationGrantor
    {
        Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default);
    }
}
