using Microsoft.AspNetCore.Authorization;

namespace Balea.Authorization.Abac
{
    internal class AbacRequirement : IAuthorizationRequirement
    {
        public AbacRequirement(string name)
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