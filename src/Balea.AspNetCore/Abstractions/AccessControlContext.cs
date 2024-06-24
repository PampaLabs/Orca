using Microsoft.Extensions.DependencyInjection;

namespace Balea
{
    public class AccessControlContext : IAccessControlContext
    {
        public IDelegationStore DelegationStore { get; }
        public IPermissionStore PermissionStore { get; }
        public IPolicyStore PolicyStore { get; }
        public IRoleStore RoleStore { get; }

        public AccessControlContext(IServiceProvider serviceProvider)
        {
            DelegationStore = serviceProvider.GetRequiredService<IDelegationStore>();
            PermissionStore = serviceProvider.GetRequiredService<IPermissionStore>();
            PolicyStore = serviceProvider.GetRequiredService<IPolicyStore>();
            RoleStore = serviceProvider.GetRequiredService<IRoleStore>();
        }
    }
}
