using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orca.Store.EntityFrameworkCore.Entities.Configuration;

internal class RoleSubjectEntityConfiguration : IEntityTypeConfiguration<RoleSubjectEntity>
{
    public void Configure(EntityTypeBuilder<RoleSubjectEntity> builder)
    {
        builder.HasKey(x => new { x.SubjectId, x.RoleId });

        builder
            .HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
