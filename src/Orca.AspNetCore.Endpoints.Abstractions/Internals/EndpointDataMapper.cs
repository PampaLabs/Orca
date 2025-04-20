namespace Orca.AspNetCore.Endpoints;

internal abstract class EndpointDataMapper<TEntity, TRequest, TResponse>
    : IEndpointDataMapper<TEntity, TRequest, TResponse>
{
    protected abstract IDataMapper<TEntity, TRequest> Request { get; }

    protected abstract IDataMapper<TEntity, TResponse> Response { get; }

    public void FromRequest(TRequest source, TEntity destination)
    {
        Request.ToEntity(source, destination);
    }

    public void ToRequest(TEntity source, TRequest destination)
    {
        Request.FromEntity(source, destination);
    }

    public void FromResponse(TResponse source, TEntity destination)
    {
        Response.ToEntity(source, destination);
    }

    public void ToResponse(TEntity source, TResponse destination)
    {
        Response.FromEntity(source, destination);
    }
}
