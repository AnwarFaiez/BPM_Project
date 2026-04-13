namespace BPM.Core.Domain.Enums;

/// <summary>
/// Workflow instance status
/// </summary>
public enum WorkflowStatus
{
    Draft,
    Active,
    Suspended,
    Completed,
    Terminated,
    Error
}

/// <summary>
/// Task status
/// </summary>
public enum TaskStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled,
    Error
}

/// <summary>
/// Process definition status
/// </summary>
public enum ProcessStatus
{
    Draft,
    Published,
    Archived
}

/// <summary>
/// Form status
/// </summary>
public enum FormStatus
{
    Draft,
    Published,
    Archived
}

/// <summary>
/// Rule status
/// </summary>
public enum RuleStatus
{
    Draft,
    Active,
    Inactive,
    Archived
}

/// <summary>
/// Integration connector types
/// </summary>
public enum ConnectorType
{
    REST,
    SOAP,
    Database,
    Email,
    SMS,
    FileSystem,
    CloudStorage,
    Custom
}

/// <summary>
/// User task priority
/// </summary>
public enum TaskPriority
{
    Low,
    Normal,
    High,
    Critical
}

/// <summary>
/// BPMN activity types
/// </summary>
public enum ActivityType
{
    StartEvent,
    EndEvent,
    IntermediateEvent,
    UserTask,
    ServiceTask,
    ScriptTask,
    BusinessRuleTask,
    SubProcess,
    CallActivity,
    ExclusiveGateway,
    ParallelGateway,
    InclusiveGateway,
    EventBasedGateway
}

/// <summary>
/// Event types
/// </summary>
public enum EventType
{
    None,
    Message,
    Timer,
    Signal,
    Error,
    Escalation,
    Cancel,
    Compensation,
    Conditional,
    Link,
    Terminate
}
