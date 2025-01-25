using System.Security.Claims;

namespace Orca
{
    public interface IPermissionEvaluator
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}
