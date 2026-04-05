using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController(IMembershipService service, IMapper mapper) : ControllerBase
    {
        private readonly IMembershipService _service = service;
        //move mapper and DTOs from API to application layer
        private readonly IMapper _mapper = mapper;

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<MembershipResponse>>> GetUserMemberships()
        {
            var result = await _service.GetUserMembershipsAsync();
            var response = _mapper.Map<List<MembershipResponse>>(result);
            return Ok(response);
        }

        [HttpGet("{organizationId}")]
        public async Task<ActionResult<MembershipResponse?>> GetMyMembershipForOrganization(int organizationId)
        {
            var result = await _service.GetUserMembershipForOrgAsync(organizationId);
            if (result == null)
                return NotFound();
            var response = _mapper.Map<MembershipResponse>(result);
            return Ok(response);
        }
        [HttpGet("{organizationId}/all")]
        public async Task<ActionResult<IEnumerable<MembershipResponse>>> GetAllMembershipsForOrganization(int organizationId)
        {
            var result = await _service.GetAllMembershipsForMyOrgAsync(organizationId);
            var response = _mapper.Map<List<MembershipResponse>>(result);
            return Ok(response);
        }
        [HttpPost("roles")]
        public async Task<ActionResult> AddMembershipRole(MembershipRequest request)
        {
            var result = await _service.AddMembershipRoleAsync(request.OrganizationId, request.UserId, request.Role);
            if (!result)
                return BadRequest();
            return CreatedAtAction(
                nameof(GetAllMembershipsForOrganization),
                new { organizationId = request.OrganizationId },
                request
            );
        }
    }
}
