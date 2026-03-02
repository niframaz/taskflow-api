using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Contracts
{
    public class TaskItemRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;
        [MaxLength(1000)]
        public string? Description { get; set; }
    }

}
