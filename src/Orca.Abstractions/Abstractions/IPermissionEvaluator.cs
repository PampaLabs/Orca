using System.Security.Claims;

namespace Orca
{
    /// <summary>
    /// Defines a method for evaluating whether a user has a specific permission.
    /// </summary>
    public interface IPermissionEvaluator
    {
        /// <summary>
        /// Asynchronously determines whether the specified user has the given permission.
        /// </summary>
        /// <param name="user">The user for whom the permission is being checked.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="bool"/> value indicating whether the user has the permission.</returns>
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}
