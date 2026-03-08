using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class MembershipResponse
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; } = default!;
        public List<OrganizationRoleDto> Roles { get; set; } = [];
    }

    public class OrganizationRoleDto
    {
        public int OrganizationRoleId { get; set; }
        public int MembershipId { get; set; }
        public OrgRole RoleName { get; set; } = default!;
    }
}
