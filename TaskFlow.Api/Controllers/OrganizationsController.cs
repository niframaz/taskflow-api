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
    public class OrganizationsController(IOrganizationService service, IMapper mapper) : ControllerBase
    {
        private readonly IOrganizationService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> Get()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
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
            var org = _mapper.Map<Organization>(request);
            var result = await _service.AddAsync(org);
            if(result)
            {
                var orgDto = _mapper.Map<OrganizationResponse>(org);
                return CreatedAtAction(nameof(Get), new { id = org.Id }, orgDto);               
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
