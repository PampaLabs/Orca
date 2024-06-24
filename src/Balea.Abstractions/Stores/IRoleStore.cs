namespace Balea
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

        Task<IList<string>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default);
        Task<AccessControlResult> AddSubjectAsync(Role role, string subject, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> RemoveSubjectAsync(Role role, string subject, CancellationToken cancellationToken = default);
    }
}
