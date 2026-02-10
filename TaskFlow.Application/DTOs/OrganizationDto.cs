namespace TaskFlow.Application.DTOs
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<ProjectDto> Projects { get; set; } = [];
        public List<MembershipDto> Memberships { get; set; } = [];
    }

    public class OrganizationSummaryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
