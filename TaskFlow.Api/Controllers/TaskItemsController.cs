using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Tasks;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskItemSummaryDto>>> GetByProject(int projectId)
        {
            var result = await _taskService.GetTasksByProjectAsync(projectId);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> Get(int id)
        {
            var result = await _taskService.GetTaskByIdAsync(id);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskItemRequest request)
        {
            var result = await _taskService.CreateTaskAsync(
                request.ProjectId,
                request.Title,
                request.Description);

            if (result.IsFailure)
            {
                return result.Error!.Contains("member")
                    ? Forbid()
                    : BadRequest(new { error = result.Error });
            }

            return CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskItemRequest request)
        {
            var result = await _taskService.UpdateTaskAsync(id, request.Title, request.Description);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (result.IsFailure)
            {
                return result.Error!.Contains("admin")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return NoContent();
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] AssignTaskRequest request)
        {
            var result = await _taskService.AssignTaskToUserAsync(id, request.UserId);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access") || result.Error!.Contains("member")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpPost("{id}/unassign")]
        public async Task<IActionResult> UnassignTask(int id)
        {
            var result = await _taskService.UnassignTaskAsync(id);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return Ok(result.Value);
        }
    }
}
