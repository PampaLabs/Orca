namespace Balea
{
    public interface IPolicyStore
    {
        Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken = default);
        Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken = default);

    	Task<AccessControlResult> CreateAsync(Policy policy, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> UpdateAsync(Policy policy, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> DeleteAsync(Policy policy, CancellationToken cancellationToken = default);

        Task<IList<Policy>> ListAsync(CancellationToken cancellationToken = default);

        Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default);
    }
}
