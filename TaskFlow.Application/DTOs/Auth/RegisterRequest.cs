using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Auth
{
    public class RegisterRequest : LoginRequest
    {
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public required string Name { get; set; }
    }
}
