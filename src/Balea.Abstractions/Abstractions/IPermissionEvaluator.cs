using System.Security.Claims;

namespace Balea
{
    public interface IPermissionEvaluator
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}
