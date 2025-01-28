namespace Orca
{
    /// <summary>
    /// Provides access to the store intances.
    /// </summary>
    public interface IOrcaStoreAccessor
    {
        /// <summary>
        /// Gets the delegation store
        /// </summary>
        IDelegationStore DelegationStore { get; }

        /// <summary>
        /// Gets the permission store
        /// </summary>
        IPermissionStore PermissionStore { get; }

        /// <summary>
        /// Gets the policy store
        /// </summary>
        IPolicyStore PolicyStore { get; }

        /// <summary>
        /// Gets the role store
        /// </summary>
        IRoleStore RoleStore { get; }

        /// <summary>
        /// Gets the subject store
        /// </summary>
        ISubjectStore SubjectStore { get; }
    }
}
