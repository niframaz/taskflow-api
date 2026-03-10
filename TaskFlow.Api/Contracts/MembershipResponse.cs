using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class MembershipResponse
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Email { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public List<OrganizationRoleDto> Roles { get; set; } = [];
    }

    public class OrganizationRoleDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrgRole Name { get; set; } = default!;
    }
}
