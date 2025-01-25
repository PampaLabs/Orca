namespace Orca.Store.Configuration;

public class SubjectStore : ISubjectStore
{
    private readonly MemoryStoreOptions _options;

	public SubjectStore(MemoryStoreOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

    public Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken)
    {
        var subject = _options.Subjects.FirstOrDefault(x => x.Id == subjectId);

        return Task.FromResult(subject);
    }

    public Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken)
	{
        var result = _options.Subjects.FirstOrDefault(x => x.Sub == sub);

        return Task.FromResult(result);
    }

	public Task<AccessControlResult> CreateAsync(Subject user, CancellationToken cancellationToken)
	{
        var subjects = _options.Subjects;

        subjects.Add(user);

        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> UpdateAsync(Subject user, CancellationToken cancellationToken)
	{
        return Task.FromResult(AccessControlResult.Success);
    }

	public Task<AccessControlResult> DeleteAsync(Subject user, CancellationToken cancellationToken)
	{
        var subjects = _options.Subjects;

        subjects.Remove(user);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<IList<Subject>> ListAsync(CancellationToken cancellationToken)
    {
        var result = _options.Subjects.ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    public Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default)
    {
        var source = _options.Subjects.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var words = filter.Name.Split().Where(word => word != string.Empty);
            source = source.Where(subject => words.All(word => subject.Name.Contains(word)));
        }

        var result = source.ToList();

        return Task.FromResult<IList<Subject>>(result);
    }

    public Task<IList<Role>> GetRolesAsync(Subject user, CancellationToken cancellationToken = default)
    {
        var result = _options.SubejctBindings
            .Where(binding => binding.Subject == user)
            .Select(binding => binding.Role)
            .ToList();

        return Task.FromResult<IList<Role>>(result);
    }

    public Task<AccessControlResult> AddRoleAsync(Subject user, Role role, CancellationToken cancellationToken)
	{
        var binding = (user, role);
        _options.SubejctBindings.Add(binding);

        return Task.FromResult(AccessControlResult.Success);
    }

    public Task<AccessControlResult> RemoveRoleAsync(Subject user, Role role, CancellationToken cancellationToken)
	{
        var binding = (user, role);
        _options.SubejctBindings.Remove(binding);

        return Task.FromResult(AccessControlResult.Success);
    }
}
