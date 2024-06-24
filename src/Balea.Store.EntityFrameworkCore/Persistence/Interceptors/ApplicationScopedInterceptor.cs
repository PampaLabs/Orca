using Balea;
using Balea.Store.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Balea.Store.EntityFrameworkCore.Interceptors;

public class ApplicationScopedInterceptor : SaveChangesInterceptor
{
    private readonly IAppContextAccessor _appContextAccessor;

    public ApplicationScopedInterceptor(IAppContextAccessor appContextAccessor)
    {
        _appContextAccessor = appContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var enties = context.ChangeTracker.Entries<IApplicationScoped>();

        if (enties.Any())
        {
            var applicationName = _appContextAccessor.AppContext.Name;

            var applicationId = context.Set<ApplicationEntity>()
                .Where(x => x.Name == applicationName)
                .Select(x => x.Id)
                .SingleOrDefault();

            foreach (var entry in enties)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.ApplicationId = applicationId;
                }
            }
        }
    }
}
