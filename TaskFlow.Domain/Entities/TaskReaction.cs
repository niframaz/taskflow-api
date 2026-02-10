namespace TaskFlow.Domain.Entities
{
    public class TaskReaction
    {
        public int Id { get; set; }
        public bool Liked { get; set; }
        public required ApplicationUser User { get; set; }
        public string UserId { get; set; } = default!;
        public required TaskItem TaskItem { get; set; }
        public int TaskItemId { get; set; }
    }
}
