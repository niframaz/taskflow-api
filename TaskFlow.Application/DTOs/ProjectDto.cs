namespace TaskFlow.Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationSummaryDto? Organization { get; set; }
        public List<TaskItemDto> TaskItems { get; set; } = [];
    }

    public class ProjectSummaryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int OrganizationId { get; set; }
    }
}
