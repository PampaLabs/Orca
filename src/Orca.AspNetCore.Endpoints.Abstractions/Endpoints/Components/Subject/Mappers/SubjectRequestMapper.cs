namespace Orca.AspNetCore.Endpoints;

internal class SubjectRequestMapper : IDataMapper<Subject, SubjectRequest>
{
    public void FromEntity(Subject source, SubjectRequest destination)
    {
        destination.Sub = source.Sub;
        destination.Email = source.Email;
        destination.Name = source.Name;
    }

    public void ToEntity(SubjectRequest source, Subject destination)
    {
        destination.Sub = source.Sub;
        destination.Email = source.Email;
        destination.Name = source.Name;
    }
}
