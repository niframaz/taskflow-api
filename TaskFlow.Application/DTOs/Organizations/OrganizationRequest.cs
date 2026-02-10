using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Organizations
{
    public class OrganizationRequest
    {
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
