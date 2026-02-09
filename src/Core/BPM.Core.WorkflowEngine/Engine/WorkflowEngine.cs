using BPM.Core.Domain.Entities;
using BPM.Core.Domain.Interfaces;
using BPM.Core.Domain.Enums;

namespace BPM.Core.WorkflowEngine.Engine;

/// <summary>
/// BPMN 2.0 workflow engine implementation
/// </summary>
public class WorkflowEngine : IWorkflowEngine
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkflowEngine(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<WorkflowInstance> StartWorkflowAsync(
        Guid processDefinitionId,
        string businessKey,
        Dictionary<string, object> variables,
        CancellationToken cancellationToken = default)
    {
        var processDefinition = await _unitOfWork.ProcessDefinitions.GetByIdAsync(processDefinitionId, cancellationToken);
        if (processDefinition == null)
        {
            throw new InvalidOperationException($"Process definition {processDefinitionId} not found");
        }

        if (processDefinition.Status != ProcessStatus.Published)
        {
            throw new InvalidOperationException($"Process definition {processDefinitionId} is not published");
        }

        var workflowInstance = new WorkflowInstance
        {
            Id = Guid.NewGuid(),
            ProcessDefinitionId = processDefinitionId,
            BusinessKey = businessKey,
            Status = WorkflowStatus.Active,
            StartedAt = DateTime.UtcNow,
            TenantId = processDefinition.TenantId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        await _unitOfWork.WorkflowInstances.AddAsync(workflowInstance, cancellationToken);

        // Add variables
        foreach (var variable in variables)
        {
            var workflowVariable = new WorkflowVariable
            {
                Id = Guid.NewGuid(),
                WorkflowInstanceId = workflowInstance.Id,
                Name = variable.Key,
                DataType = variable.Value?.GetType().Name ?? "string",
                Value = variable.Value?.ToString() ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            await _unitOfWork.WorkflowInstances.AddAsync(workflowInstance, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Start execution
        await ContinueWorkflowAsync(workflowInstance.Id, cancellationToken);

        return workflowInstance;
    }

    public async Task ContinueWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _unitOfWork.WorkflowInstances.GetByIdAsync(workflowInstanceId, cancellationToken);
        if (workflowInstance == null)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} not found");
        }

        if (workflowInstance.Status != WorkflowStatus.Active)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} is not active");
        }

        // Parse BPMN and execute
        // This is a simplified implementation - would parse and execute BPMN XML
        // For now, we just mark as completed
        workflowInstance.Status = WorkflowStatus.Completed;
        workflowInstance.CompletedAt = DateTime.UtcNow;
        workflowInstance.UpdatedAt = DateTime.UtcNow;
        workflowInstance.UpdatedBy = "System";

        await _unitOfWork.WorkflowInstances.UpdateAsync(workflowInstance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteTaskAsync(
        Guid taskInstanceId,
        Dictionary<string, object> variables,
        CancellationToken cancellationToken = default)
    {
        var taskInstance = await _unitOfWork.TaskInstances.GetByIdAsync(taskInstanceId, cancellationToken);
        if (taskInstance == null)
        {
            throw new InvalidOperationException($"Task instance {taskInstanceId} not found");
        }

        if (taskInstance.Status == Domain.Enums.TaskStatus.Completed)
        {
            throw new InvalidOperationException($"Task instance {taskInstanceId} is already completed");
        }

        taskInstance.Status = Domain.Enums.TaskStatus.Completed;
        taskInstance.CompletedAt = DateTime.UtcNow;
        taskInstance.UpdatedAt = DateTime.UtcNow;
        taskInstance.UpdatedBy = "System";

        await _unitOfWork.TaskInstances.UpdateAsync(taskInstance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Continue workflow
        await ContinueWorkflowAsync(taskInstance.WorkflowInstanceId, cancellationToken);
    }

    public async Task SuspendWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _unitOfWork.WorkflowInstances.GetByIdAsync(workflowInstanceId, cancellationToken);
        if (workflowInstance == null)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} not found");
        }

        workflowInstance.Status = WorkflowStatus.Suspended;
        workflowInstance.UpdatedAt = DateTime.UtcNow;
        workflowInstance.UpdatedBy = "System";

        await _unitOfWork.WorkflowInstances.UpdateAsync(workflowInstance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ResumeWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _unitOfWork.WorkflowInstances.GetByIdAsync(workflowInstanceId, cancellationToken);
        if (workflowInstance == null)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} not found");
        }

        if (workflowInstance.Status != WorkflowStatus.Suspended)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} is not suspended");
        }

        workflowInstance.Status = WorkflowStatus.Active;
        workflowInstance.UpdatedAt = DateTime.UtcNow;
        workflowInstance.UpdatedBy = "System";

        await _unitOfWork.WorkflowInstances.UpdateAsync(workflowInstance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await ContinueWorkflowAsync(workflowInstanceId, cancellationToken);
    }

    public async Task TerminateWorkflowAsync(Guid workflowInstanceId, string reason, CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _unitOfWork.WorkflowInstances.GetByIdAsync(workflowInstanceId, cancellationToken);
        if (workflowInstance == null)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} not found");
        }

        workflowInstance.Status = WorkflowStatus.Terminated;
        workflowInstance.CompletedAt = DateTime.UtcNow;
        workflowInstance.UpdatedAt = DateTime.UtcNow;
        workflowInstance.UpdatedBy = "System";

        await _unitOfWork.WorkflowInstances.UpdateAsync(workflowInstance, cancellationToken);
        
        // Log event
        var workflowEvent = new WorkflowEvent
        {
            Id = Guid.NewGuid(),
            WorkflowInstanceId = workflowInstanceId,
            EventType = "Terminated",
            EventName = "Workflow Terminated",
            OccurredAt = DateTime.UtcNow,
            EventData = reason,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SignalWorkflowAsync(
        Guid workflowInstanceId,
        string signalName,
        Dictionary<string, object>? data = null,
        CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _unitOfWork.WorkflowInstances.GetByIdAsync(workflowInstanceId, cancellationToken);
        if (workflowInstance == null)
        {
            throw new InvalidOperationException($"Workflow instance {workflowInstanceId} not found");
        }

        // Log signal event
        var workflowEvent = new WorkflowEvent
        {
            Id = Guid.NewGuid(),
            WorkflowInstanceId = workflowInstanceId,
            EventType = "Signal",
            EventName = signalName,
            OccurredAt = DateTime.UtcNow,
            EventData = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : null,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Continue workflow if waiting for this signal
        if (workflowInstance.Status == WorkflowStatus.Active)
        {
            await ContinueWorkflowAsync(workflowInstanceId, cancellationToken);
        }
    }
}
