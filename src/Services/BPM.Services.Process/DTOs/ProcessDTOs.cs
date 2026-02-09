namespace BPM.Services.Process.DTOs;

/// <summary>
/// Request to create a process definition
/// </summary>
public class CreateProcessDefinitionRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string BpmnXml { get; set; } = string.Empty;
    public string? Category { get; set; }
}

/// <summary>
/// Response with process definition details
/// </summary>
public class ProcessDefinitionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request to start a workflow instance
/// </summary>
public class StartWorkflowRequest
{
    public string BusinessKey { get; set; } = string.Empty;
    public Dictionary<string, object> Variables { get; set; } = new();
}

/// <summary>
/// Response with workflow instance details
/// </summary>
public class WorkflowInstanceResponse
{
    public Guid Id { get; set; }
    public Guid ProcessDefinitionId { get; set; }
    public string BusinessKey { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? CurrentActivityId { get; set; }
}

/// <summary>
/// Request to complete a task
/// </summary>
public class CompleteTaskRequest
{
    public Dictionary<string, object> Variables { get; set; } = new();
}

/// <summary>
/// Response with task instance details
/// </summary>
public class TaskInstanceResponse
{
    public Guid Id { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
