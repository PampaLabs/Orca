namespace Orca
{
    /// <summary>
    /// Provides an abstraction for a store which manages permissions.
    /// </summary>
    public interface IPermissionStore
    {
        /// <summary>
        /// Finds and returns a permission, if any, who has the specified id.
        /// </summary>
        /// <param name="permissionId">The permission ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the permission matching the specified <paramref name="permissionId"/> if it exists.</returns>
        Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds and returns a permission, if any, who has the specified name.
        /// </summary>
        /// <param name="permissionName">The permission name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the permission matching the specified <paramref name="permissionName"/> if it exists.</returns>
        Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified <paramref name="permission"/> in the store.
        /// </summary>
        /// <param name="permission">The permission to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the creation operation.</returns>
        Task<AccessManagementResult> CreateAsync(Permission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified <paramref name="permission"/> in the store.
        /// </summary>
        /// <param name="permission">The permission to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the update operation.</returns>
        Task<AccessManagementResult> UpdateAsync(Permission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified <paramref name="permission"/> from the store.
        /// </summary>
        /// <param name="permission">The permission to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the delete operation.</returns>
        Task<AccessManagementResult> DeleteAsync(Permission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the permissions in the store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Permission"/> list.</returns>
        Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Search the permissions in the store, that match the specified <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The permission filter to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Permission"/> list.</returns>
        Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the roles for the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission whose roles should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Role"/> list for the specified <paramref name="permission"/>.</returns>
        Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the <paramref name="role"/> for the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission to which the role will be added.</param>
        /// <param name="role">The role to be added.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the add operation.</returns>
        Task<AccessManagementResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the <paramref name="role"/> for the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission to which the role will be removed.</param>
        /// <param name="role">The role to be removeed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the remove operation.</returns>
        Task<AccessManagementResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken = default);
    }
}
