using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

internal class DelegationMapper : IEntityMapper<DelegationEntity, Delegation>
{
    public void FromEntity(DelegationEntity source, Delegation destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(Delegation source, DelegationEntity destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
