namespace Balea.Store.Configuration;

internal class DelegationMapper : IEntityMapper<DelegationConfiguration, Delegation>
{
    public void FromEntity(DelegationConfiguration source, Delegation destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
    }

    public void ToEntity(Delegation source, DelegationConfiguration destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
    }
}
