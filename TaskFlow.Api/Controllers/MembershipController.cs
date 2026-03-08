using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController(IMembershipService service) : ControllerBase
    {
        private readonly IMembershipService _service = service;
        // GET: api/<MembershipController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetUserMembershipsAsync();
            return Ok(result);
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
