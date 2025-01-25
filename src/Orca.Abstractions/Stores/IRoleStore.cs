namespace Orca
{
    public interface IRoleStore
    {
        Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = default);
        Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken = default);

    	Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken = default);

        Task<IList<Role>> ListAsync(CancellationToken cancellationToken = default);

        Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default);

        Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default);
        Task<AccessControlResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken = default);

        Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default);
        Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken = default);
        Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken = default);
    }
}
