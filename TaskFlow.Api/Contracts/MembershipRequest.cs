using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class MembershipRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required int OrganizationId { get; set; }
        [EnumDataType(typeof(UserRole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrgRole Role { get; set; }
    }
}
