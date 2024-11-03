namespace Balea
{
    public interface ISubjectStore
    {
        Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken = default);
        Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken = default);

        Task<AccessControlResult> CreateAsync(Subject subject, CancellationToken cancellationToken = default);
        Task<AccessControlResult> UpdateAsync(Subject subject, CancellationToken cancellationToken = default);
        Task<AccessControlResult> DeleteAsync(Subject subject, CancellationToken cancellationToken = default);

        Task<IList<Subject>> ListAsync(CancellationToken cancellationToken = default);

        Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default);

        Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default);
        Task<AccessControlResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken = default);
        Task<AccessControlResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken = default);

    }
}
