using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orca.Store.EntityFrameworkCore.Entities.Configuration;

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

        builder.Property(x => x.Enabled)
            .IsRequired();

        builder
            .HasOne(x => x.Who)
            .WithMany()
            .HasForeignKey(x => x.WhoId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(x => x.Whom)
            .WithMany()
            .HasForeignKey(x => x.WhomId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasIndex(x => new { x.From, x.To });
    }
}
