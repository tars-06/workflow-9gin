using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Services;
using WorkflowEngine.DTOs;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine.Controllers
{
    [ApiController]
    [Route("api/workflow-instances")]
    public class WorkflowInstanceController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowInstanceController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        public async Task<IActionResult> StartInstance([FromBody] StartWorkflowInstanceDto dto)
        {
            try
            {
                var instance = await _workflowService.StartInstanceAsync(dto);
                return CreatedAtAction(nameof(GetInstance), new { id = instance.Id }, instance);
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstance(string id)
        {
            try
            {
                var instance = await _workflowService.GetInstanceAsync(id);
                return Ok(instance);
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInstances()
        {
            try
            {
                var instances = await _workflowService.GetAllInstancesAsync();
                return Ok(instances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecuteAction(string id, [FromBody] ExecuteActionDto dto)
        {
            try
            {
                var instance = await _workflowService.ExecuteActionAsync(id, dto);
                return Ok(instance);
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidStateTransitionException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (WorkflowValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("{id}/available-actions")]
        public async Task<IActionResult> GetAvailableActions(string id)
        {
            try
            {
                var actions = await _workflowService.GetAvailableActionsAsync(id);
                return Ok(actions);
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("{id}/current-state")]
        public async Task<IActionResult> GetCurrentState(string id)
        {
            try
            {
                var state = await _workflowService.GetCurrentStateAsync(id);
                return Ok(state);
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }
    }
}
