using Microsoft.Extensions.DependencyInjection;

namespace Orca
{
    /// <inheritdoc />
    public class OrcaStoreAccessor : IOrcaStoreAccessor
    {
        /// <inheritdoc />
        public IDelegationStore DelegationStore { get; }

        /// <inheritdoc />
        public IPermissionStore PermissionStore { get; }

        /// <inheritdoc />
        public IPolicyStore PolicyStore { get; }

        /// <inheritdoc />
        public IRoleStore RoleStore { get; }

        /// <inheritdoc />
        public ISubjectStore SubjectStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrcaStoreAccessor"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies for each store.</param>
        public OrcaStoreAccessor(IServiceProvider serviceProvider)
        {
            DelegationStore = serviceProvider.GetRequiredService<IDelegationStore>();
            PermissionStore = serviceProvider.GetRequiredService<IPermissionStore>();
            PolicyStore = serviceProvider.GetRequiredService<IPolicyStore>();
            RoleStore = serviceProvider.GetRequiredService<IRoleStore>();
            SubjectStore = serviceProvider.GetRequiredService<ISubjectStore>();
        }
    }
}
