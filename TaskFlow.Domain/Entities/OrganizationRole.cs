using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class OrganizationRole
    {
        public int Id { get; set; }
        public required OrgRoles Role { get; set; }
        public OrganizationMembership OrganizationUserRole { get; set; } = default!;
        public int OrganizationMembershipId { get; set; }
    }
}
