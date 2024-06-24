using Microsoft.AspNetCore.Authorization;

namespace Balea.Authorization.Rbac
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}