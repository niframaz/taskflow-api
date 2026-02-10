namespace TaskFlow.Application.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public UserDto? User { get; set; }
        public int ProjectId { get; set; }
        public ProjectSummaryDto? Project { get; set; }
    }

    public class TaskItemSummaryDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
