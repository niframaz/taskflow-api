using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class UserRoleCreationRequest
    {
        [EnumDataType(typeof(UserRole))]
        public required UserRole Role { get; set; }
    }
}
