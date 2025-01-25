namespace Orca.AspNetCore.Endpoints;

internal class DelegationRequestMapper : IEntityMapper<Delegation, DelegationRequest>
{
    public void FromEntity(Delegation source, DelegationRequest destination)
    {
        destination.Who = source.Who.Id;
        destination.Whom = source.Whom.Id;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(DelegationRequest source, Delegation destination)
    {
        destination.Who = new () { Id = source.Who };
        destination.Whom = new () { Id = source.Whom };
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
