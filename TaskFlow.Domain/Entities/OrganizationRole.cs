using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class OrganizationRole
    {
        public int Id { get; set; }
        public required OrgRole Role { get; set; }
        public OrganizationMembership OrganizationMembership { get; set; } = default!;
        public int OrganizationMembershipId { get; set; }
    }
}
