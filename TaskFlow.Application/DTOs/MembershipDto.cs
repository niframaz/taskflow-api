using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs
{
    public class MembershipDto
    {
        public int OrganizationId { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
