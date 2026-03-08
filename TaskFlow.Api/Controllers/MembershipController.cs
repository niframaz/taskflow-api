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
    public class MembershipController(IMembershipService service, IMapper mapper) : ControllerBase
    {
        private readonly IMembershipService _service = service;
        private readonly IMapper _mapper = mapper;
        // GET: api/<MembershipController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> Get()
        {
            var result = await _service.GetUserMembershipsAsync();
            var response = _mapper.Map<List<MembershipResponse>>(result);
            return Ok(response);
        }

        // GET api/<MembershipController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MembershipController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MembershipController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MembershipController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
