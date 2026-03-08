using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class UserRoleCreationRequest
    {
        [EnumDataType(typeof(UserRole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required UserRole Role { get; set; }
    }
}
