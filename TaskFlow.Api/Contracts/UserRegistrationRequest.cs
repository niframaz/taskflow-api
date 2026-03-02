using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class UserRegistrationRequest : UserLoginRequest
    {
        public required string Name { get; set; }
        [Range(1, int.MaxValue)]
        public int OrganizationId { get; set; }
    }
}
