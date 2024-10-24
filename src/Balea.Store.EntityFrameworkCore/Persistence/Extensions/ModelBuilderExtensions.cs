using Balea.Store.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Balea.Store.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void HasApplicationFilter(this ModelBuilder modelBuilder, IAppContextAccessor appContextAccessor)
    {
        if (appContextAccessor is not null)
        {
            modelBuilder.Entity<ApplicationEntity>().HasQueryFilter(x => x.Name == appContextAccessor.AppContext.Name);

            var entityTypes = modelBuilder.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                if (typeof(IApplicationScoped).IsAssignableFrom(entityType.ClrType))
                {
                    var lambda = CreateLambdaFilter(entityType.ClrType, appContextAccessor);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }

    private static LambdaExpression CreateLambdaFilter(Type clrType, IAppContextAccessor appContextAccessor)
    {
        var parameter = Expression.Parameter(clrType, "x");
        var applicationProperty = Expression.Property(parameter, nameof(IApplicationScoped.Application));
        var nameProperty = Expression.Property(applicationProperty, nameof(IApplicationScoped.Application.Name));
        var appContextName = Expression.Constant(appContextAccessor.AppContext.Name);
        var equality = Expression.Equal(nameProperty, appContextName);

        var lambda = Expression.Lambda(equality, parameter);

        return lambda;
    }
}
