namespace Orca.AspNetCore.Endpoints;

internal interface IDataMapper<TEntity, TModel>
{
    void FromEntity(TEntity source, TModel destination);

    void ToEntity(TModel source, TEntity destination);
}
