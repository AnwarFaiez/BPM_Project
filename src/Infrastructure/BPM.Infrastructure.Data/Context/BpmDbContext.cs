using Microsoft.EntityFrameworkCore;
using BPM.Core.Domain.Entities;

namespace BPM.Infrastructure.Data.Context;

/// <summary>
/// Main database context for the BPM system
/// </summary>
public class BpmDbContext : DbContext
{
    public BpmDbContext(DbContextOptions<BpmDbContext> options) : base(options)
    {
    }

    // Workflow entities
    public DbSet<ProcessDefinition> ProcessDefinitions { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<TaskInstance> TaskInstances { get; set; }
    public DbSet<ProcessVariable> ProcessVariables { get; set; }
    public DbSet<WorkflowVariable> WorkflowVariables { get; set; }
    public DbSet<TaskVariable> TaskVariables { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<WorkflowEvent> WorkflowEvents { get; set; }

    // Form entities
    public DbSet<FormDefinition> FormDefinitions { get; set; }
    public DbSet<FormSubmission> FormSubmissions { get; set; }

    // Rule entities
    public DbSet<RuleDefinition> RuleDefinitions { get; set; }

    // Identity entities
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BpmDbContext).Assembly);

        // Global query filters for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BPM.Core.Domain.Common.BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BPM.Core.Domain.Common.BaseEntity.IsDeleted));
                var filter = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
                    parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BPM.Core.Domain.Common.BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.Id = Guid.NewGuid();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
