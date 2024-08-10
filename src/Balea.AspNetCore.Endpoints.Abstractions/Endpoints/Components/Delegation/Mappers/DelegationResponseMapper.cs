namespace Balea.AspNetCore.Endpoints;

internal class DelegationResponseMapper : IEntityMapper<Delegation, DelegationResponse>
{
    public void FromEntity(Delegation source, DelegationResponse destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(DelegationResponse source, Delegation destination)
    {
        destination.Id = source.Id;
        destination.Who = source.Who;
        destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
