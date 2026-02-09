using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BPM.Core.Domain.Entities;

namespace BPM.Infrastructure.Data.Configurations;

public class ProcessDefinitionConfiguration : IEntityTypeConfiguration<ProcessDefinition>
{
    public void Configure(EntityTypeBuilder<ProcessDefinition> builder)
    {
        builder.ToTable("ProcessDefinitions");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Key).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.BpmnXml).IsRequired();
        
        builder.HasIndex(p => new { p.Key, p.VersionNumber }).IsUnique();
        builder.HasIndex(p => p.TenantId);
    }
}

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
    {
        builder.ToTable("WorkflowInstances");
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.BusinessKey).IsRequired().HasMaxLength(200);
        
        builder.HasOne(w => w.ProcessDefinition)
            .WithMany(p => p.Instances)
            .HasForeignKey(w => w.ProcessDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(w => w.BusinessKey);
        builder.HasIndex(w => w.Status);
        builder.HasIndex(w => w.TenantId);
    }
}

public class TaskInstanceConfiguration : IEntityTypeConfiguration<TaskInstance>
{
    public void Configure(EntityTypeBuilder<TaskInstance> builder)
    {
        builder.ToTable("TaskInstances");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.ActivityId).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(1000);
        
        builder.HasOne(t => t.WorkflowInstance)
            .WithMany(w => w.Tasks)
            .HasForeignKey(t => t.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.FormDefinition)
            .WithMany()
            .HasForeignKey(t => t.FormDefinitionId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.AssignedTo);
        builder.HasIndex(t => t.AssignedGroup);
    }
}

public class FormDefinitionConfiguration : IEntityTypeConfiguration<FormDefinition>
{
    public void Configure(EntityTypeBuilder<FormDefinition> builder)
    {
        builder.ToTable("FormDefinitions");
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Name).IsRequired().HasMaxLength(200);
        builder.Property(f => f.Key).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Description).HasMaxLength(1000);
        builder.Property(f => f.FormJson).IsRequired();
        
        builder.HasIndex(f => new { f.Key, f.VersionNumber }).IsUnique();
        builder.HasIndex(f => f.TenantId);
    }
}

public class FormSubmissionConfiguration : IEntityTypeConfiguration<FormSubmission>
{
    public void Configure(EntityTypeBuilder<FormSubmission> builder)
    {
        builder.ToTable("FormSubmissions");
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.SubmittedBy).IsRequired().HasMaxLength(200);
        builder.Property(f => f.FormData).IsRequired();
        
        builder.HasOne(f => f.FormDefinition)
            .WithMany(d => d.Submissions)
            .HasForeignKey(f => f.FormDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(f => f.TaskInstance)
            .WithMany()
            .HasForeignKey(f => f.TaskInstanceId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(f => f.SubmittedAt);
    }
}

public class RuleDefinitionConfiguration : IEntityTypeConfiguration<RuleDefinition>
{
    public void Configure(EntityTypeBuilder<RuleDefinition> builder)
    {
        builder.ToTable("RuleDefinitions");
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Key).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Description).HasMaxLength(1000);
        builder.Property(r => r.DmnXml).IsRequired();
        
        builder.HasIndex(r => new { r.Key, r.VersionNumber }).IsUnique();
        builder.HasIndex(r => r.TenantId);
    }
}
