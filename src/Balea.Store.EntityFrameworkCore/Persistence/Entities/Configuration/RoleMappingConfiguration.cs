using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.Store.EntityFrameworkCore.Entities.Configuration;

internal class RoleMappingEntityConfiguration : IEntityTypeConfiguration<RoleMappingEntity>
{
    public void Configure(EntityTypeBuilder<RoleMappingEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.Mapping, x.RoleId })
            .IsUnique();

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
