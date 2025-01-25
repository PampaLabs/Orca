using Orca.Store.EntityFrameworkCore.Entities;

namespace Orca.Store.EntityFrameworkCore;

internal class SubjectMapper : IEntityMapper<SubjectEntity, Subject>
{
    public void FromEntity(SubjectEntity source, Subject destination)
    {
        destination.Id = source.Id;
        destination.Sub = source.Sub;
        destination.Email = source.Email;
        destination.Name = source.Name;
    }

    public void ToEntity(Subject source, SubjectEntity destination)
    {
        destination.Id = source.Id;
        destination.Sub = source.Sub;
        destination.Email = source.Email;
        destination.Name = source.Name;
    }
}
