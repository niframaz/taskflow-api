using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.Memberships
{
    public class MembershipResponse
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public required string Email { get; set; }
        public required string UserId { get; set; }
        public List<OrganizationRoleDto> Roles { get; set; } = [];
    }

    public class OrganizationRoleDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrgRole Name { get; set; }
    }
}
