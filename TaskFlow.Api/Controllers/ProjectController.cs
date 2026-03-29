using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService, IMapper mapper) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            var result = await _projectService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            var result = await _projectService.GetAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
            return result;
        }

        [HttpPost("{organizationId}")]
        public async Task<IActionResult> Post([FromBody] ProjectRequest projectRequest, [FromRoute] int organizationId)
        {
            var request = _mapper.Map<Project>(projectRequest);
            request.OrganizationId = organizationId;
            var result = await _projectService.AddAsync(request);
            if (result)
                return CreatedAtAction(nameof(Get), new { id = request.Id }, request);
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectRequest projectRequest, [FromRoute] int organizationId)
        {
            var request = _mapper.Map<Project>(projectRequest);
            request.OrganizationId = organizationId;
            var result = await _projectService.UpdateAsync(id, request);
            if (result)
                return NoContent();
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.RemoveAsync(id);
            if (result)
                return NoContent();
            return StatusCode(500);
        }
    }
}
