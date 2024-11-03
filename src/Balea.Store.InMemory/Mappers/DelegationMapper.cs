namespace Balea.Store.Configuration;

internal class DelegationMapper : IEntityMapper<DelegationConfiguration, Delegation>
{
    public void FromEntity(DelegationConfiguration source, Delegation destination)
    {
        destination.Who = new () { Sub = source.Who };
        destination.Who = new () { Sub = source.Whom };
        destination.From = source.From;
        destination.To = source.To;
    }

    public void ToEntity(Delegation source, DelegationConfiguration destination)
    {
        destination.Who = source.Who.Sub;
        destination.Whom = source.Whom.Sub;
        destination.From = source.From;
        destination.To = source.To;
    }
}
