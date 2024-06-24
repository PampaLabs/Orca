using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

internal class PolicyMapper : IEntityMapper<PolicyEntity, Policy>
{
    public void FromEntity(PolicyEntity source, Policy destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Content = source.Content;
    }

    public void ToEntity(Policy source, PolicyEntity destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Content = source.Content;
    }
}
