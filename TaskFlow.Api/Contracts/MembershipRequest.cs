using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class MembershipRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required int OrganizationId { get; set; }
        [EnumDataType(typeof(OrgRole))]
        public required OrgRole Role { get; set; }
    }
}
