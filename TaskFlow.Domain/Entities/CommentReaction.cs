using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class CommentReaction
    {
        public int Id { get; set; }
        public Reaction Reaction { get; set; }
        public required Comment Comment { get; set; }
        public int CommentId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }
    }
}
