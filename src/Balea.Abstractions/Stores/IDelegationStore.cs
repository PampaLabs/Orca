namespace Balea
{
    public interface IDelegationStore
    {
    	Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken = default);
        Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);

        Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken = default);
    	Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken = default);

        Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken = default);

        Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken = default);
    }
}
