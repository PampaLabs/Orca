namespace Orca.Store.Configuration;

internal class SubjectMapper : IEntityMapper<SubjectConfiguration, Subject>
{
    public void FromEntity(SubjectConfiguration source, Subject destination)
    {
        destination.Sub = source.Sub;
        destination.Name = source.Name;
        destination.Email = source.Email;
    }

    public void ToEntity(Subject source, SubjectConfiguration destination)
    {
        destination.Sub = source.Sub;
        destination.Name = source.Name;
        destination.Email = source.Email;
    }
}
