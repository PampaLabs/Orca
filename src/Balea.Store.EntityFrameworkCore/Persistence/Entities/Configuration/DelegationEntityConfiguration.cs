using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.Store.EntityFrameworkCore.Entities.Configuration;

internal class DelegationEntityConfiguration : IEntityTypeConfiguration<DelegationEntity>
{
    public void Configure(EntityTypeBuilder<DelegationEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.From)
            .IsRequired();

        builder.Property(x => x.To)
            .IsRequired();

        builder.Property(x => x.Who)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Whom)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Enabled)
            .IsRequired();

        builder.HasIndex(x => x.Whom);

        builder.HasIndex(x => new { x.From, x.To });
    }
}
