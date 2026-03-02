using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Contracts
{
    public class UserRoleAssignmentRequest : UserRoleCreationRequest
    {
        public required string Email { get; set; }
        
    }
}
