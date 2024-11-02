namespace Balea.Store.Configuration;

internal static class ModelExtensions
{
    public static Delegation GetCurrentDelegation(this IEnumerable<Delegation> delegations, string subjectId)
    {
        return delegations.FirstOrDefault(d => d.Active && d.Whom == subjectId);
    }

    public static Application GetByName(this IEnumerable<Application> applications, string name)
    {
        return applications.First(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}
