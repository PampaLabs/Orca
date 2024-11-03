namespace Balea.AspNetCore.Endpoints;

internal class DelegationResponseMapper : IEntityMapper<Delegation, DelegationResponse>
{
    public void FromEntity(Delegation source, DelegationResponse destination)
    {
        var subjectMapper = new SubjectResponseMapper();

        destination.Id = source.Id;
        destination.Who = subjectMapper.FromEntity(source.Who);
        destination.Whom = subjectMapper.FromEntity(source.Whom);
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(DelegationResponse source, Delegation destination)
    {
        var subjectMapper = new SubjectResponseMapper();

        destination.Id = source.Id;
        destination.Who = subjectMapper.ToEntity(source.Who);
        destination.Whom = subjectMapper.ToEntity(source.Whom);
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
