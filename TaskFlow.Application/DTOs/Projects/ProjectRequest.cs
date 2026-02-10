using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Projects
{
    public class ProjectRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public required int OrganizationId { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
