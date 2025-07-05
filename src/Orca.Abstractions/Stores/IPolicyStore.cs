namespace Orca
{
    /// <summary>
    /// Provides an abstraction for a store which manages policies.
    /// </summary>
    public interface IPolicyStore
    {
        /// <summary>
        /// Finds and returns a policy, if any, who has the specified id.
        /// </summary>
        /// <param name="policyId">The policy ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the policy matching the specified <paramref name="policyId"/> if it exists.</returns>
        Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds and returns a policy, if any, who has the specified name.
        /// </summary>
        /// <param name="policyName">The policy name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the policy matching the specified <paramref name="policyName"/> if it exists.</returns>
        Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified <paramref name="policy"/> in the store.
        /// </summary>
        /// <param name="policy">The policy to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the creation operation.</returns>
        Task<AccessManagementResult> CreateAsync(Policy policy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified <paramref name="policy"/> in the store.
        /// </summary>
        /// <param name="policy">The policy to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the update operation.</returns>
        Task<AccessManagementResult> UpdateAsync(Policy policy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified <paramref name="policy"/> from the store.
        /// </summary>
        /// <param name="policy">The policy to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="AccessManagementResult"/> of the delete operation.</returns>
        Task<AccessManagementResult> DeleteAsync(Policy policy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the policies in the store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Policy"/> list.</returns>
        Task<IList<Policy>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Search the policies in the store, that match the specified <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The policy filter to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Policy"/> list.</returns>
        Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default);
    }
}
