using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Organizations;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController(IOrganizationService service) : ControllerBase
    {
        private readonly IOrganizationService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationSummaryDto>>> Get()
        {
            var result = await _service.GetMyOrganizationsAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> Get(int id)
        {
            var result = await _service.GetOrganizationByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound(new { error = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationRequest request)
        {
            var result = await _service.CreateOrganizationAsync(request.Name, request.Description);
            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error });
            }

            return CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrganizationRequest request)
        {
            var result = await _service.UpdateOrganizationAsync(id, request.Name, request.Description);
            if (result.IsFailure)
            {
                return result.Error!.Contains("not an admin")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteOrganizationAsync(id);
            if (result.IsFailure)
            {
                return result.Error!.Contains("not an admin")
                    ? Forbid()
                    : NotFound(new { error = result.Error });
            }

            return NoContent();
        }
    }
}
