using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public Reaction Reaction { get; set; }
        public TaskItem TaskItem { get; set; } = default!;
        public int TaskItemId { get; set; }
    }
}
