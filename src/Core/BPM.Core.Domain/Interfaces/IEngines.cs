using BPM.Core.Domain.Entities;

namespace BPM.Core.Domain.Interfaces;

/// <summary>
/// Workflow engine interface for BPMN 2.0 execution
/// </summary>
public interface IWorkflowEngine
{
    /// <summary>
    /// Starts a new workflow instance
    /// </summary>
    Task<WorkflowInstance> StartWorkflowAsync(Guid processDefinitionId, string businessKey, Dictionary<string, object> variables, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Continues workflow execution
    /// </summary>
    Task ContinueWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Completes a task and moves workflow forward
    /// </summary>
    Task CompleteTaskAsync(Guid taskInstanceId, Dictionary<string, object> variables, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Suspends a workflow instance
    /// </summary>
    Task SuspendWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Resumes a suspended workflow
    /// </summary>
    Task ResumeWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Terminates a workflow instance
    /// </summary>
    Task TerminateWorkflowAsync(Guid workflowInstanceId, string reason, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Signals a waiting workflow
    /// </summary>
    Task SignalWorkflowAsync(Guid workflowInstanceId, string signalName, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Forms engine interface for dynamic form handling
/// </summary>
public interface IFormsEngine
{
    /// <summary>
    /// Renders a form definition
    /// </summary>
    Task<object> RenderFormAsync(Guid formDefinitionId, Dictionary<string, object>? initialData = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates form data
    /// </summary>
    Task<ValidationResult> ValidateFormDataAsync(Guid formDefinitionId, Dictionary<string, object> formData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Submits form data
    /// </summary>
    Task<FormSubmission> SubmitFormAsync(Guid formDefinitionId, Dictionary<string, object> formData, Guid? taskInstanceId = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Rules engine interface for DMN execution
/// </summary>
public interface IRulesEngine
{
    /// <summary>
    /// Evaluates a decision rule
    /// </summary>
    Task<Dictionary<string, object>> EvaluateRuleAsync(Guid ruleDefinitionId, Dictionary<string, object> input, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates rule definition
    /// </summary>
    Task<ValidationResult> ValidateRuleAsync(Guid ruleDefinitionId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Integration service interface
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// Executes a connector
    /// </summary>
    Task<object> ExecuteConnectorAsync(string connectorType, Dictionary<string, object> parameters, CancellationToken cancellationToken = default);
}

/// <summary>
/// Validation result
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}
