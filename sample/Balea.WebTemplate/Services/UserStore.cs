namespace Balea.WebTemplate.Services;

public class UserStore : IUserStore
{
    private readonly IList<User> _users = TestUsers.Users;

    public Task<User> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(x => x.SubjectId == subject);

        return Task.FromResult(user);
    }

    public Task<User> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(x => x.Username == username);

        return Task.FromResult(user);
    }

    public Task<AccessControlResult> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AccessControlResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AccessControlResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IList<User>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users);
    }

    public Task<IList<User>> SearchAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _users.AsQueryable();

        if (!string.IsNullOrEmpty(filter.StartsWith))
        {
            query = query.Where(x => x.Username.StartsWith(filter.StartsWith));
        }

        var results = query.ToList();

        return Task.FromResult<IList<User>>(results);
    }
}
