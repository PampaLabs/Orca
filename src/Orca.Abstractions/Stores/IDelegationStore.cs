namespace Orca
{
    /// <summary>
    /// Provides an abstraction for a store which manages delegations.
    /// </summary>
    public interface IDelegationStore
    {
        /// <summary>
        /// Finds and returns a delegation, if any, who has the specified id.
        /// </summary>
        /// <param name="delegationId">The delegation ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the delegation matching the specified <paramref name="delegationId"/> if it exists.</returns>
        Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds and returns a delegation, if any, who has the specified subject.
        /// </summary>
        /// <param name="subject">The delegation subject to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the delegation matching the specified <paramref name="subject"/> if it exists.</returns>
        Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified <paramref name="delegation"/> in the store.
        /// </summary>
        /// <param name="delegation">The delegation to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the creation operation.</returns>
        Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified <paramref name="delegation"/> in the store.
        /// </summary>
        /// <param name="delegation">The delegation to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the update operation.</returns>
        Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified <paramref name="delegation"/> from the store.
        /// </summary>
        /// <param name="delegation">The delegation to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessControlResult"/> of the delete operation.</returns>
        Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the delegations in the store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Delegation"/> list.</returns>
        Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Search the delegations in the store, that match the specified <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The delegation filter to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Delegation"/> list.</returns>
        Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken = default);
    }
}
