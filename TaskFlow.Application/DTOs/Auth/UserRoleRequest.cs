using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.Auth
{
    public class UserRoleRequest
    {
        [Required]
        [EnumDataType(typeof(UserRole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required UserRole Role { get; set; }
    }
}
