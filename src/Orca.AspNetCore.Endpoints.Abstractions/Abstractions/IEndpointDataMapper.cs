namespace Orca.AspNetCore.Endpoints;

internal interface IEndpointDataMapper<TEntity, TRequest, TResponse>
{
    void FromRequest(TRequest source, TEntity destination);
    void ToRequest(TEntity source, TRequest destination);

    void FromResponse(TResponse source, TEntity destination);
    void ToResponse(TEntity source, TResponse destination);
}
