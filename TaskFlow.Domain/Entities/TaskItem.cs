using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public required string Title { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public bool Liked { get; set; }
        public ICollection<Comment> Comments { get; set; } = [];
    }
}
