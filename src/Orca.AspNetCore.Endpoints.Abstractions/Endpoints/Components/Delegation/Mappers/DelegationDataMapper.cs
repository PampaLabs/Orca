namespace Orca.AspNetCore.Endpoints;

internal class DelegationDataMapper : EndpointDataMapper<Delegation, DelegationRequest, DelegationResponse>
{
    protected override IDataMapper<Delegation, DelegationRequest> Request { get; }

    protected override IDataMapper<Delegation, DelegationResponse> Response { get; }

    public DelegationDataMapper()
    {
        Request = new DelegationRequestMapper();
        Response = new DelegationResponseMapper();
    }
}
