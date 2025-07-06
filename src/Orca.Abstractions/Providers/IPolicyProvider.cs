namespace Orca
{
    /// <summary>
    /// Defines a service that provides <see cref="Policy"/> instances by name.
    /// </summary>
    public interface IPolicyProvider
    {
        /// <summary>
        /// Retrieves a policy by its name.
        /// </summary>
        /// <param name="name">The name of the policy to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the requested <see cref="Policy"/>.</returns>
        Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default);
    }
}
