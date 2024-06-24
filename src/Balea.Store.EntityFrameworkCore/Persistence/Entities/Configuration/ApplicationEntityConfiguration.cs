using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.Store.EntityFrameworkCore.Entities.Configuration;

internal class ApplicationEntityConfiguration : IEntityTypeConfiguration<ApplicationEntity>
{
    private readonly IAppContextAccessor _appContextAccessor;

    public ApplicationEntityConfiguration(IAppContextAccessor appContextAccessor)
    {
        _appContextAccessor = appContextAccessor;
    }

    public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.ImageUrl)
            .IsRequired(false);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasQueryFilter(x => x.Name == _appContextAccessor.AppContext.Name);
    }
}
