namespace Orca.AspNetCore.Endpoints;

internal class DelegationResponseMapper : IDataMapper<Delegation, DelegationResponse>
{
    private readonly SubjectResponseMapper _subjectMapper = new();

    public void FromEntity(Delegation source, DelegationResponse destination)
    {
        destination.Id = source.Id;
        destination.Who = _subjectMapper.FromEntity(source.Who);
        destination.Whom = _subjectMapper.FromEntity(source.Whom);
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }

    public void ToEntity(DelegationResponse source, Delegation destination)
    {
        destination.Id = source.Id;
        destination.Who = _subjectMapper.ToEntity(source.Who);
        destination.Whom = _subjectMapper.ToEntity(source.Whom);
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;
    }
}
