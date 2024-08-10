namespace Balea.AspNetCore.Endpoints;

internal class DelegationRequestMapper : IEntityMapper<Delegation, DelegationRequest>
{
    public void FromEntity(Delegation source, DelegationRequest destination)
    {
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(DelegationRequest source, Delegation destination)
    {
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
