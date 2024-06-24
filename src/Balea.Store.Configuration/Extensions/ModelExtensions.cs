namespace Balea.Store.Configuration;

public static class ModelExtensions
{
	public static DelegationConfiguration GetCurrentDelegation(this IEnumerable<DelegationConfiguration> delegations, string subjectId)
	{
		return delegations.FirstOrDefault(d => d.Active && d.Whom == subjectId);
	}

    public static ApplicationConfiguration GetByName(this IEnumerable<ApplicationConfiguration> applications, string name)
	{
		return applications.First(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
	}
}
