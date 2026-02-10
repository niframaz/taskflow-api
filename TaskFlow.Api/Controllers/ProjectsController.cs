using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Projects;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSummaryDto>>> Get()
        {
            var result = await _projectService.GetMyProjectsAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound(new { error = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpGet("organizations/{organizationId}")]
        public async Task<ActionResult<IEnumerable<ProjectSummaryDto>>> GetByOrganization(int organizationId)
        {
            var result = await _projectService.GetProjectsByOrganizationAsync(organizationId);
            if (result.IsFailure)
            {
                return result.Error!.Contains("access")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectRequest request)
        {
            var result = await _projectService.CreateProjectAsync(
                request.OrganizationId,
                request.Name,
                request.Description);

            if (result.IsFailure)
            {
                return result.Error!.Contains("admin")
                    ? Forbid()
                    : BadRequest(new { error = result.Error });
            }

            return CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectRequest request)
        {
            var result = await _projectService.UpdateProjectAsync(id, request.Name, request.Description);
            if (result.IsFailure)
            {
                return result.Error!.Contains("admin")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (result.IsFailure)
            {
                return result.Error!.Contains("admin")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return NoContent();
        }
    }
}
