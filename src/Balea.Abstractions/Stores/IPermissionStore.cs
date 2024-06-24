namespace Balea
{
    public interface IPermissionStore
    {
        Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken = default);
        Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken = default);

    	Task<AccessControlResult> CreateAsync(Permission permission, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> UpdateAsync(Permission permission, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> DeleteAsync(Permission permission, CancellationToken cancellationToken = default);

        Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default);

        Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default);

        Task<IList<string>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default);
        Task<AccessControlResult> AddRoleAsync(Permission permission, string roleName, CancellationToken cancellationToken = default);
        Task<AccessControlResult> RemoveRoleAsync(Permission permission, string roleName, CancellationToken cancellationToken = default);
    }
}
