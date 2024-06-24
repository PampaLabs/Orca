using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.Store.EntityFrameworkCore.Entities.Configuration;

internal class RoleSubjectEntityConfiguration : IEntityTypeConfiguration<RoleSubjectEntity>
{
    public void Configure(EntityTypeBuilder<RoleSubjectEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.Sub, x.RoleId })
            .IsUnique();

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
