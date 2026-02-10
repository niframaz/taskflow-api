using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Tasks
{
    public class AssignTaskRequest
    {
        [Required]
        [MaxLength(450)]
        public required string UserId { get; set; }
    }
}
