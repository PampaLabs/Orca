namespace Orca.AspNetCore.Endpoints;

internal class PolicyDataMapper : EndpointDataMapper<Policy, PolicyRequest, PolicyResponse>
{
    protected override IDataMapper<Policy, PolicyRequest> Request { get; }

    protected override IDataMapper<Policy, PolicyResponse> Response { get; }

    public PolicyDataMapper()
    {
        Request = new PolicyRequestMapper();
        Response = new PolicyResponseMapper();
    }
}
