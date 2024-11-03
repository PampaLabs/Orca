using Balea.Store.EntityFrameworkCore.Entities;

namespace Balea.Store.EntityFrameworkCore;

internal class DelegationMapper : IEntityMapper<DelegationEntity, Delegation>
{
    public void FromEntity(DelegationEntity source, Delegation destination)
    {
        destination.Id = source.Id;
        // destination.Who = source.Who;
        // destination.Whom = source.Whom;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;

        var subjectMapper = new SubjectMapper();
        destination.Who = subjectMapper.FromEntity(source.Who);
        destination.Whom = subjectMapper.FromEntity(source.Whom);
    }

    public void ToEntity(Delegation source, DelegationEntity destination)
    {
        destination.Id = source.Id;
        destination.WhoId = source.Who.Id;
        destination.WhomId = source.Whom.Id;
        destination.From = source.From;
        destination.To = source.To;
        destination.Enabled = source.Enabled;

        // var subjectMapper = new SubjectMapper();
        // destination.Who = subjectMapper.ToEntity(source.Who);
        // destination.Whom = subjectMapper.ToEntity(source.Whom);
    }
}
