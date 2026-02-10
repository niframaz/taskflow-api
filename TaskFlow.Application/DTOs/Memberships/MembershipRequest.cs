using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.Memberships
{
    public class MembershipRequest
    {
        [Required]
        [MaxLength(450)]
        public required string UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public required int OrganizationId { get; set; }

        [Required]
        [EnumDataType(typeof(OrgRole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrgRole Role { get; set; }
    }
}
