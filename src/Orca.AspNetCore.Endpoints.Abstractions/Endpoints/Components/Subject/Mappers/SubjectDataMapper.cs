namespace Orca.AspNetCore.Endpoints;

internal class SubjectDataMapper : EndpointDataMapper<Subject, SubjectRequest, SubjectResponse>
{
    protected override IDataMapper<Subject, SubjectRequest> Request { get; }

    protected override IDataMapper<Subject, SubjectResponse> Response { get; }

    public SubjectDataMapper()
    {
        Request = new SubjectRequestMapper();
        Response = new SubjectResponseMapper();
    }
}
