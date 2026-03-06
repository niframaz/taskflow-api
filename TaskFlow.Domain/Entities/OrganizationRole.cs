using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class OrganizationRole
    {
        public int Id { get; set; }
        public required OrgRoles Role { get; set; }
        public OrganizationUserRole OrganizationUserRole { get; set; } = default!;
        public int OrganizationUserRoleId { get; set; }
    }
}
