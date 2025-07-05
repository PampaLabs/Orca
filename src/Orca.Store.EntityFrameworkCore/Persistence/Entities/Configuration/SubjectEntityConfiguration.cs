using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orca.Store.EntityFrameworkCore.Entities.Configuration;

internal class SubjectEntityConfiguration : IEntityTypeConfiguration<SubjectEntity>
{
    public void Configure(EntityTypeBuilder<SubjectEntity> builder)
    {
        builder.ToTable("Subjects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
             .HasMaxLength(50)
             .IsRequired();

        builder.Property(x => x.Sub)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(x => x.Sub)
            .IsUnique();
    }
}
