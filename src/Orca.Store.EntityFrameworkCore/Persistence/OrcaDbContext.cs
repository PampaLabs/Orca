using Orca.Store.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Orca.Store.EntityFrameworkCore;

/// <summary>
/// Represents the database context for interacting with authorization entities.
/// </summary>
public class OrcaDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet for <see cref="SubjectEntity"/>.
    /// </summary>
    public DbSet<SubjectEntity> Subjects { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="RoleEntity"/>.
    /// </summary>
    public DbSet<RoleEntity> Roles { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="PermissionEntity"/>.
    /// </summary>
    public DbSet<PermissionEntity> Permissions { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="DelegationEntity"/>.
    /// </summary>
    public DbSet<DelegationEntity> Delegations { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="PolicyEntity"/>.
    /// </summary>
    public DbSet<PolicyEntity> Policies { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="RolePermissionEntity"/>.
    /// </summary>
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="RoleMappingEntity"/>.
    /// </summary>
    public DbSet<RoleMappingEntity> RoleMappings { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="RoleSubjectEntity"/>.
    /// </summary>
    public DbSet<RoleSubjectEntity> RoleSubjects { get; set; }

    /// <inheritdoc />
    public OrcaDbContext(DbContextOptions options)
        : base(options)
	{
	}

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrcaDbContext).Assembly);
    }
}
