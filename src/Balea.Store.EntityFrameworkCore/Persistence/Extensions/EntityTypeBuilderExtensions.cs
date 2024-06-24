using Balea.Store.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.Store.EntityFrameworkCore;

internal static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> IsApplicationScoped<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IApplicationScoped
    {
        builder.HasOne(x => x.Application)
            .WithMany()
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => x.ApplicationId);

        return builder;
    }
}
