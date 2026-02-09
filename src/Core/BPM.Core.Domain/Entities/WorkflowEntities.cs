using BPM.Core.Domain.Common;
using BPM.Core.Domain.Enums;

namespace BPM.Core.Domain.Entities;

/// <summary>
/// Represents a workflow process definition (BPMN 2.0)
/// </summary>
public class ProcessDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public ProcessStatus Status { get; set; }
    public string BpmnXml { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? TenantId { get; set; }
    
    // Navigation properties
    public ICollection<WorkflowInstance> Instances { get; set; } = new List<WorkflowInstance>();
    public ICollection<ProcessVariable> Variables { get; set; } = new List<ProcessVariable>();
}

/// <summary>
/// Represents a running instance of a workflow process
/// </summary>
public class WorkflowInstance : BaseEntity
{
    public Guid ProcessDefinitionId { get; set; }
    public string BusinessKey { get; set; } = string.Empty;
    public WorkflowStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? CurrentActivityId { get; set; }
    public string? TenantId { get; set; }
    public string? InitiatedBy { get; set; }
    
    // Navigation properties
    public ProcessDefinition ProcessDefinition { get; set; } = null!;
    public ICollection<TaskInstance> Tasks { get; set; } = new List<TaskInstance>();
    public ICollection<WorkflowVariable> Variables { get; set; } = new List<WorkflowVariable>();
    public ICollection<WorkflowEvent> Events { get; set; } = new List<WorkflowEvent>();
}

/// <summary>
/// Represents a task instance in a workflow
/// </summary>
public class TaskInstance : BaseEntity
{
    public Guid WorkflowInstanceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ActivityId { get; set; } = string.Empty;
    public ActivityType ActivityType { get; set; }
    public Enums.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public string? AssignedGroup { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? FormDefinitionId { get; set; }
    
    // Navigation properties
    public WorkflowInstance WorkflowInstance { get; set; } = null!;
    public FormDefinition? FormDefinition { get; set; }
    public ICollection<TaskVariable> Variables { get; set; } = new List<TaskVariable>();
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
}

/// <summary>
/// Represents a form definition
/// </summary>
public class FormDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public FormStatus Status { get; set; }
    public string FormJson { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? TenantId { get; set; }
    
    // Navigation properties
    public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
}

/// <summary>
/// Represents a form submission
/// </summary>
public class FormSubmission : BaseEntity
{
    public Guid FormDefinitionId { get; set; }
    public Guid? TaskInstanceId { get; set; }
    public Guid? WorkflowInstanceId { get; set; }
    public string SubmittedBy { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string FormData { get; set; } = string.Empty;
    
    // Navigation properties
    public FormDefinition FormDefinition { get; set; } = null!;
    public TaskInstance? TaskInstance { get; set; }
}

/// <summary>
/// Represents a business rule definition (DMN)
/// </summary>
public class RuleDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public RuleStatus Status { get; set; }
    public string DmnXml { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? TenantId { get; set; }
}

/// <summary>
/// Represents a process variable
/// </summary>
public class ProcessVariable : BaseEntity
{
    public Guid ProcessDefinitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string? DefaultValue { get; set; }
    public bool IsRequired { get; set; }
    
    // Navigation properties
    public ProcessDefinition ProcessDefinition { get; set; } = null!;
}

/// <summary>
/// Represents a workflow variable instance
/// </summary>
public class WorkflowVariable : BaseEntity
{
    public Guid WorkflowInstanceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    
    // Navigation properties
    public WorkflowInstance WorkflowInstance { get; set; } = null!;
}

/// <summary>
/// Represents a task variable
/// </summary>
public class TaskVariable : BaseEntity
{
    public Guid TaskInstanceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    
    // Navigation properties
    public TaskInstance TaskInstance { get; set; } = null!;
}

/// <summary>
/// Represents a task comment
/// </summary>
public class TaskComment : BaseEntity
{
    public Guid TaskInstanceId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string CommentedBy { get; set; } = string.Empty;
    public DateTime CommentedAt { get; set; }
    
    // Navigation properties
    public TaskInstance TaskInstance { get; set; } = null!;
}

/// <summary>
/// Represents a workflow event
/// </summary>
public class WorkflowEvent : BaseEntity
{
    public Guid WorkflowInstanceId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string? EventData { get; set; }
    
    // Navigation properties
    public WorkflowInstance WorkflowInstance { get; set; } = null!;
}
