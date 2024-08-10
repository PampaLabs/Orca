namespace Balea.AspNetCore.Endpoints;

internal interface IEntityMapper<TEntity, TModel>
{
    void FromEntity(TEntity source, TModel destination);

    void ToEntity(TModel source, TEntity destination);
}

internal static class EntityMapperExtensions
{
    public static TModel FromEntity<TEntity, TModel>(this IEntityMapper<TEntity, TModel> mapper, TEntity source)
        where TModel : class, new()
    {
        if (source is null) return null;

        var destination = new TModel();
        mapper.FromEntity(source, destination);
        return destination;
    }

    public static TEntity ToEntity<TEntity, TModel>(this IEntityMapper<TEntity, TModel> mapper, TModel source)
        where TEntity : class, new()
    {
        if (source is null) return null;

        var destination = new TEntity();
        mapper.ToEntity(source, destination);
        return destination;
    }
}
