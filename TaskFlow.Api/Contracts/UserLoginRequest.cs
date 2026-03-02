using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Contracts
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
