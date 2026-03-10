using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class MembershipRequest
    {
        public required string UserId { get; set; }
        public required int OrganizationId { get; set; }
        [EnumDataType(typeof(OrgRole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrgRole Role { get; set; }
    }
}
