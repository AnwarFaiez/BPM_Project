using Microsoft.AspNetCore.Mvc;
using BPM.Core.Domain.Interfaces;
using BPM.Services.Process.DTOs;

namespace BPM.Services.Process.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkflowEngine _workflowEngine;
    private readonly ILogger<TasksController> _logger;

    public TasksController(
        IUnitOfWork unitOfWork,
        IWorkflowEngine workflowEngine,
        ILogger<TasksController> logger)
    {
        _unitOfWork = unitOfWork;
        _workflowEngine = workflowEngine;
        _logger = logger;
    }

    /// <summary>
    /// Get task instance by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskInstanceResponse>> GetById(Guid id)
    {
        try
        {
            var task = await _unitOfWork.TaskInstances.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task instance with ID {id} not found");
            }

            var response = new TaskInstanceResponse
            {
                Id = task.Id,
                WorkflowInstanceId = task.WorkflowInstanceId,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                DueDate = task.DueDate,
                AssignedTo = task.AssignedTo,
                StartedAt = task.StartedAt,
                CompletedAt = task.CompletedAt
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task instance {TaskId}", id);
            return StatusCode(500, "An error occurred while retrieving the task instance");
        }
    }

    /// <summary>
    /// Complete a task instance
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult> Complete(Guid id, CompleteTaskRequest request)
    {
        try
        {
            await _workflowEngine.CompleteTaskAsync(id, request.Variables);
            return Ok(new { message = "Task completed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing task {TaskId}", id);
            return StatusCode(500, "An error occurred while completing the task");
        }
    }
}
