namespace Orca
{
    /// <summary>
    /// Provides an abstraction for a store which manages subjects.
    /// </summary>
    public interface ISubjectStore
    {
        /// <summary>
        /// Finds and returns a subject, if any, who has the specified id.
        /// </summary>
        /// <param name="subjectId">The subject ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the subject matching the specified <paramref name="subjectId"/> if it exists.</returns>
        Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds and returns a subject, if any, who has the specified sub.
        /// </summary>
        /// <param name="sub">The subject sub to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the subject matching the specified <paramref name="sub"/> if it exists.</returns>
        Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified <paramref name="subject"/> in the store.
        /// </summary>
        /// <param name="subject">The subject to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the creation operation.</returns>
        Task<AccessManagementResult> CreateAsync(Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified <paramref name="subject"/> in the store.
        /// </summary>
        /// <param name="subject">The subject to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the update operation.</returns>
        Task<AccessManagementResult> UpdateAsync(Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified <paramref name="subject"/> from the store.
        /// </summary>
        /// <param name="subject">The subject to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the delete operation.</returns>
        Task<AccessManagementResult> DeleteAsync(Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the subjects in the store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Subject"/> list.</returns>
        Task<IList<Subject>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Search the subjects in the store, that match the specified <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The subject filter to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Subject"/> list.</returns>
        Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the roles for the specified <paramref name="subject"/>.
        /// </summary>
        /// <param name="subject">The subject whose roles should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Role"/> list for the specified <paramref name="subject"/>.</returns>
        Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the <paramref name="role"/> for the specified <paramref name="subject"/>.
        /// </summary>
        /// <param name="subject">The subject to which the role will be added.</param>
        /// <param name="role">The role to be added.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the add operation.</returns>
        Task<AccessManagementResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the <paramref name="role"/> for the specified <paramref name="subject"/>.
        /// </summary>
        /// <param name="subject">The subject to which the role will be removed.</param>
        /// <param name="role">The role to be removeed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the remove operation.</returns>
        Task<AccessManagementResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken = default);

    }
}
