using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Tasks
{
    public class TaskItemRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public required int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
