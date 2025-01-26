namespace Orca
{
    /// <summary>
    /// Provides an abstraction for a store which manages roles.
    /// </summary>
    public interface IRoleStore
    {
        /// <summary>
        /// Finds and returns a role, if any, who has the specified id.
        /// </summary>
        /// <param name="roleId">The role ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role matching the specified <paramref name="roleId"/> if it exists.</returns>
        Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds and returns a role, if any, who has the specified name.
        /// </summary>
        /// <param name="roleName">The role name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role matching the specified <paramref name="roleName"/> if it exists.</returns>
        Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified <paramref name="role"/> in the store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the creation operation.</returns>
        Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified <paramref name="role"/> in the store.
        /// </summary>
        /// <param name="role">The role to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the update operation.</returns>
        Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified <paramref name="role"/> from the store.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the delete operation.</returns>
        Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the roles in the store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Role"/> list.</returns>
        Task<IList<Role>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Search the roles in the store, that match the specified <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The role filter to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Role"/> list.</returns>
        Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the subjects for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose subjects should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Subject"/> list for the specified <paramref name="role"/>.</returns>
        Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the <paramref name="subject"/> for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to which the subject will be added.</param>
        /// <param name="subject">The subject to be added.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the add operation.</returns>
        Task<AccessControlResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the <paramref name="subject"/> for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to which the subject will be removed.</param>
        /// <param name="subject">The subject to be removeed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the remove operation.</returns>
        Task<AccessControlResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the permissions for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose permissions should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Permission"/> list for the specified <paramref name="role"/>.</returns>
        Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the <paramref name="permission"/> for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to which the permission will be added.</param>
        /// <param name="permission">The permission to be added.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the add operation.</returns>
        Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the <paramref name="permission"/> for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to which the permission will be removed.</param>
        /// <param name="permission">The permission to be removeed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the remove operation.</returns>
        Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken = default);
    }
}
