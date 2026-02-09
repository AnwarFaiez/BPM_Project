using Microsoft.AspNetCore.Mvc;
using BPM.Core.Domain.Interfaces;
using BPM.Services.Process.DTOs;

namespace BPM.Services.Process.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkflowEngine _workflowEngine;
    private readonly ILogger<WorkflowsController> _logger;

    public WorkflowsController(
        IUnitOfWork unitOfWork,
        IWorkflowEngine workflowEngine,
        ILogger<WorkflowsController> logger)
    {
        _unitOfWork = unitOfWork;
        _workflowEngine = workflowEngine;
        _logger = logger;
    }

    /// <summary>
    /// Get workflow instance by ID
    /// </summary>
    [HttpGet("{id}", Name = "GetWorkflowById")]
    public async Task<ActionResult<WorkflowInstanceResponse>> GetById(Guid id)
    {
        try
        {
            var workflow = await _unitOfWork.WorkflowInstances.GetByIdAsync(id);
            if (workflow == null)
            {
                return NotFound($"Workflow instance with ID {id} not found");
            }

            var response = new WorkflowInstanceResponse
            {
                Id = workflow.Id,
                ProcessDefinitionId = workflow.ProcessDefinitionId,
                BusinessKey = workflow.BusinessKey,
                Status = workflow.Status.ToString(),
                StartedAt = workflow.StartedAt,
                CompletedAt = workflow.CompletedAt,
                CurrentActivityId = workflow.CurrentActivityId
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow instance {WorkflowId}", id);
            return StatusCode(500, "An error occurred while retrieving the workflow instance");
        }
    }

    /// <summary>
    /// Suspend a workflow instance
    /// </summary>
    [HttpPost("{id}/suspend")]
    public async Task<ActionResult> Suspend(Guid id)
    {
        try
        {
            await _workflowEngine.SuspendWorkflowAsync(id);
            return Ok(new { message = "Workflow suspended successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error suspending workflow {WorkflowId}", id);
            return StatusCode(500, "An error occurred while suspending the workflow");
        }
    }

    /// <summary>
    /// Resume a suspended workflow instance
    /// </summary>
    [HttpPost("{id}/resume")]
    public async Task<ActionResult> Resume(Guid id)
    {
        try
        {
            await _workflowEngine.ResumeWorkflowAsync(id);
            return Ok(new { message = "Workflow resumed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resuming workflow {WorkflowId}", id);
            return StatusCode(500, "An error occurred while resuming the workflow");
        }
    }

    /// <summary>
    /// Terminate a workflow instance
    /// </summary>
    [HttpPost("{id}/terminate")]
    public async Task<ActionResult> Terminate(Guid id, [FromBody] string reason)
    {
        try
        {
            await _workflowEngine.TerminateWorkflowAsync(id, reason);
            return Ok(new { message = "Workflow terminated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error terminating workflow {WorkflowId}", id);
            return StatusCode(500, "An error occurred while terminating the workflow");
        }
    }
}
