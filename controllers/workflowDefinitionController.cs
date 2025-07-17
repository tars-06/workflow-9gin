using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Services;
using WorkflowEngine.DTOs;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine.Controllers
{
    [ApiController]
    [Route("api/workflow-definitions")]
    public class WorkflowDefinitionController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowDefinitionController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDefinition([FromBody] CreateWorkflowDefinitionDto dto)
        {
            try
            {
                var definition = await _workflowService.CreateDefinitionAsync(dto);
                return CreatedAtAction(nameof(GetDefinition), new { id = definition.Id }, definition);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDefinition(string id)
        {
            try
            {
                var definition = await _workflowService.GetDefinitionAsync(id);
                return Ok(definition);
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
        public async Task<IActionResult> GetAllDefinitions()
        {
            try
            {
                var definitions = await _workflowService.GetAllDefinitionsAsync();
                return Ok(definitions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefinition(string id)
        {
            try
            {
                var deleted = await _workflowService.DeleteDefinitionAsync(id);
                if (deleted)
                    return NoContent();
                return NotFound();
            }
            catch (WorkflowNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
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

        [HttpGet("{id}/instances")]
        public async Task<IActionResult> GetInstancesByDefinition(string id)
        {
            try
            {
                var instances = await _workflowService.GetInstancesByDefinitionIdAsync(id);
                return Ok(instances);
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
