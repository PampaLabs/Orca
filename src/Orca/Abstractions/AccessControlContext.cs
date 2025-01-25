using Microsoft.Extensions.DependencyInjection;

namespace Orca
{
    public class AccessControlContext : IAccessControlContext
    {
        public IDelegationStore DelegationStore { get; }
        public IPermissionStore PermissionStore { get; }
        public IPolicyStore PolicyStore { get; }
        public IRoleStore RoleStore { get; }
        public ISubjectStore SubjectStore { get; }

        public AccessControlContext(IServiceProvider serviceProvider)
        {
            DelegationStore = serviceProvider.GetRequiredService<IDelegationStore>();
            PermissionStore = serviceProvider.GetRequiredService<IPermissionStore>();
            PolicyStore = serviceProvider.GetRequiredService<IPolicyStore>();
            RoleStore = serviceProvider.GetRequiredService<IRoleStore>();
            SubjectStore = serviceProvider.GetRequiredService<ISubjectStore>();
        }
    }
}
