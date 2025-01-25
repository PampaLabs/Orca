using Orca.Store.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

public class OrcaDbContext : DbContext
{
    public DbSet<SubjectEntity> Subjects { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
	public DbSet<PermissionEntity> Permissions { get; set; }
	public DbSet<DelegationEntity> Delegations { get; set; }
	public DbSet<PolicyEntity> Policies { get; set; }
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }
    public DbSet<RoleMappingEntity> RoleMappings { get; set; }
    public DbSet<RoleSubjectEntity> RoleSubjects { get; set; }

    public OrcaDbContext(DbContextOptions options)
        : base(options)
	{
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrcaDbContext).Assembly);
    }
}
