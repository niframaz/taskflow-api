using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class MembershipController(IMembershipService service, IMapper mapper) : ControllerBase
    {
        private readonly IMembershipService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet("my-memberships")]
        public async Task<ActionResult<IEnumerable<MembershipResponse>>> Get()
        {
            var result = await _service.GetUserMembershipsAsync();
            var response = _mapper.Map<List<MembershipResponse>>(result);
            return Ok(response);
        }

        [HttpGet("my-membership-for-org/{organizationId}")]
        public async Task<ActionResult<MembershipResponse?>> Get(int organizationId)
        {
            var result = await _service.GetUserMembershipForOrgAsync(organizationId);
            if (result == null)
                return NotFound();
            var response = _mapper.Map<MembershipResponse>(result);
            return Ok(response);
        }
        [HttpGet("all-memberships-for-org/{organizationId}")]
        public async Task<ActionResult<IEnumerable<MembershipResponse>>> GetAllForOrg(int organizationId)
        {
            var result = await _service.GetAllMembershipsForMyOrgAsync(organizationId);
            var response = _mapper.Map<List<MembershipResponse>>(result);
            return Ok(response);
        }
    }
}
