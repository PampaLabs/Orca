namespace Orca.AspNetCore.Endpoints;

internal static class DataMapperExtensions
{
    public static TModel FromEntity<TEntity, TModel>(this IDataMapper<TEntity, TModel> mapper, TEntity source)
        where TModel : class, new()
    {
        if (source is null) return null;

        var destination = new TModel();
        mapper.FromEntity(source, destination);
        return destination;
    }

    public static TEntity ToEntity<TEntity, TModel>(this IDataMapper<TEntity, TModel> mapper, TModel source)
        where TEntity : class, new()
    {
        if (source is null) return null;

        var destination = new TEntity();
        mapper.ToEntity(source, destination);
        return destination;
    }
}
