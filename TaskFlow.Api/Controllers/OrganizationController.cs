using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController(IOrganizationService service, IMapper mapper) : ControllerBase
    {
        private readonly IOrganizationService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> Get()
        {
            var result = await _service.GetAllAsync();
            return Ok(result); ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> Get(int id)
        {
            var result = await _service.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationRequest request)
        {
            var result = _mapper.Map<Organization>(request);
            var created = await _service.AddAsync(result);
            if(created)
            {
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrganizationRequest request)
        {
            var result = _mapper.Map<Organization>(request);
            result.Id = id;
            var updated = await _service.UpdateAsync(id, result);
            if (updated)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.RemoveAsync(id);
            if (deleted)
            {
                return NoContent();
            }
            return StatusCode(500);
        }
    }
}
