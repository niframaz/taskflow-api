using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public required string Name { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = default!;
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<TaskItem>? TaskItems { get; set; }
    }
}
