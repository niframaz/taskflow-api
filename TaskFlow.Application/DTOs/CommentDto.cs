namespace TaskFlow.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public int TaskItemId { get; set; }
        public string? UserId { get; set; }
        public UserDto? User { get; set; }
        public List<CommentReactionDto> CommentReactions { get; set; } = [];
    }

    public class CommentSummaryDto
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class CommentReactionDto
    {
        public int Id { get; set; }
        public string Reaction { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class TaskReactionDto
    {
        public int Id { get; set; }
        public string Reaction { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
