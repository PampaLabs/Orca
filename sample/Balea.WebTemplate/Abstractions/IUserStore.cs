namespace Balea.WebTemplate;

public interface IUserStore
{
    Task<User> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);
    Task<User> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<AccessControlResult> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<AccessControlResult> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<AccessControlResult> DeleteAsync(User user, CancellationToken cancellationToken = default);

    Task<IList<User>> ListAsync(CancellationToken cancellationToken = default);

    Task<IList<User>> SearchAsync(UserFilter filter, CancellationToken cancellationToken = default);
}
