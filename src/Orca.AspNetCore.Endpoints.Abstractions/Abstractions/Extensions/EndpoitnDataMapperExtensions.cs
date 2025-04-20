namespace Orca.AspNetCore.Endpoints;

internal static class EndpointDataMapperExtensions
{
    public static TEntity FromRequest<TEntity, TRequest, TResponse>(this IEndpointDataMapper<TEntity, TRequest, TResponse> mapper, TRequest source)
        where TEntity : class, new()
    {
        if (source is null) return null;

        var destination = new TEntity();
        mapper.FromRequest(source, destination);
        return destination;
    }

    public static TRequest ToRequest<TEntity, TRequest, TResponse>(this IEndpointDataMapper<TEntity, TRequest, TResponse> mapper, TEntity source)
        where TRequest : class, new()
    {
        if (source is null) return null;

        var destination = new TRequest();
        mapper.ToRequest(source, destination);
        return destination;
    }

    public static TEntity FromResponse<TEntity, TRequest, TResponse>(this IEndpointDataMapper<TEntity, TRequest, TResponse> mapper, TResponse source)
        where TEntity : class, new()
    {
        if (source is null) return null;

        var destination = new TEntity();
        mapper.FromResponse(source, destination);
        return destination;
    }

    public static TResponse ToResponse<TEntity, TRequest, TResponse>(this IEndpointDataMapper<TEntity, TRequest, TResponse> mapper, TEntity source)
        where TResponse : class, new()
    {
        if (source is null) return null;

        var destination = new TResponse();
        mapper.ToResponse(source, destination);
        return destination;
    }
}
