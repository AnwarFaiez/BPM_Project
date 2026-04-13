using Microsoft.AspNetCore.Mvc;
using BPM.Core.Domain.Entities;
using BPM.Core.Domain.Interfaces;
using BPM.Core.Domain.Enums;
using BPM.Services.Process.DTOs;

namespace BPM.Services.Process.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessesController> _logger;

    public ProcessesController(IUnitOfWork unitOfWork, ILogger<ProcessesController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Get all process definitions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessDefinitionResponse>>> GetAll()
    {
        try
        {
            var processes = await _unitOfWork.ProcessDefinitions.GetAllAsync();
            var response = processes.Select(p => new ProcessDefinitionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Key = p.Key,
                VersionNumber = p.VersionNumber,
                Status = p.Status.ToString(),
                Category = p.Category,
                CreatedAt = p.CreatedAt
            });
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving process definitions");
            return StatusCode(500, "An error occurred while retrieving process definitions");
        }
    }

    /// <summary>
    /// Get a specific process definition by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProcessDefinitionResponse>> GetById(Guid id)
    {
        try
        {
            var process = await _unitOfWork.ProcessDefinitions.GetByIdAsync(id);
            if (process == null)
            {
                return NotFound($"Process definition with ID {id} not found");
            }

            var response = new ProcessDefinitionResponse
            {
                Id = process.Id,
                Name = process.Name,
                Description = process.Description,
                Key = process.Key,
                VersionNumber = process.VersionNumber,
                Status = process.Status.ToString(),
                Category = process.Category,
                CreatedAt = process.CreatedAt
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving process definition {ProcessId}", id);
            return StatusCode(500, "An error occurred while retrieving the process definition");
        }
    }

    /// <summary>
    /// Create a new process definition
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProcessDefinitionResponse>> Create(CreateProcessDefinitionRequest request)
    {
        try
        {
            var process = new ProcessDefinition
            {
                Name = request.Name,
                Description = request.Description,
                Key = request.Key,
                VersionNumber = 1,
                Status = ProcessStatus.Draft,
                BpmnXml = request.BpmnXml,
                Category = request.Category,
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ProcessDefinitions.AddAsync(process);
            await _unitOfWork.SaveChangesAsync();

            var response = new ProcessDefinitionResponse
            {
                Id = process.Id,
                Name = process.Name,
                Description = process.Description,
                Key = process.Key,
                VersionNumber = process.VersionNumber,
                Status = process.Status.ToString(),
                Category = process.Category,
                CreatedAt = process.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = process.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating process definition");
            return StatusCode(500, "An error occurred while creating the process definition");
        }
    }

    /// <summary>
    /// Publish a process definition
    /// </summary>
    [HttpPost("{id}/publish")]
    public async Task<ActionResult> Publish(Guid id)
    {
        try
        {
            var process = await _unitOfWork.ProcessDefinitions.GetByIdAsync(id);
            if (process == null)
            {
                return NotFound($"Process definition with ID {id} not found");
            }

            process.Status = ProcessStatus.Published;
            process.UpdatedAt = DateTime.UtcNow;
            process.UpdatedBy = "System";

            await _unitOfWork.ProcessDefinitions.UpdateAsync(process);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Process definition published successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing process definition {ProcessId}", id);
            return StatusCode(500, "An error occurred while publishing the process definition");
        }
    }

    /// <summary>
    /// Start a new workflow instance
    /// </summary>
    [HttpPost("{id}/start")]
    public async Task<ActionResult<WorkflowInstanceResponse>> StartWorkflow(Guid id, StartWorkflowRequest request)
    {
        try
        {
            var process = await _unitOfWork.ProcessDefinitions.GetByIdAsync(id);
            if (process == null)
            {
                return NotFound($"Process definition with ID {id} not found");
            }

            if (process.Status != ProcessStatus.Published)
            {
                return BadRequest("Process definition must be published before starting an instance");
            }

            var workflowInstance = new WorkflowInstance
            {
                ProcessDefinitionId = id,
                BusinessKey = request.BusinessKey,
                Status = WorkflowStatus.Active,
                StartedAt = DateTime.UtcNow,
                InitiatedBy = "System",
                TenantId = process.TenantId,
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WorkflowInstances.AddAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            var response = new WorkflowInstanceResponse
            {
                Id = workflowInstance.Id,
                ProcessDefinitionId = workflowInstance.ProcessDefinitionId,
                BusinessKey = workflowInstance.BusinessKey,
                Status = workflowInstance.Status.ToString(),
                StartedAt = workflowInstance.StartedAt
            };

            return CreatedAtAction("GetWorkflowById", "Workflows", new { id = workflowInstance.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting workflow for process {ProcessId}", id);
            return StatusCode(500, "An error occurred while starting the workflow");
        }
    }
}
