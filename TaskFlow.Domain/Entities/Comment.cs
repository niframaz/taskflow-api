namespace TaskFlow.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public ICollection<CommentReaction> CommentReactions { get; set; } = [];
        public required TaskItem TaskItem { get; set; }
        public int TaskItemId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }
    }
}
