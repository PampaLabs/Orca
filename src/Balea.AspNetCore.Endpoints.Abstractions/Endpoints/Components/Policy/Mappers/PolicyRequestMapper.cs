namespace Balea.AspNetCore.Endpoints;

internal class PolicyRequestMapper : IEntityMapper<Policy, PolicyRequest>
{
    public void FromEntity(Policy source, PolicyRequest destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Content = source.Content;
    }

    public void ToEntity(PolicyRequest source, Policy destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Content = source.Content;
    }
}
