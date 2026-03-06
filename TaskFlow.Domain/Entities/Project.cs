using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public required Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = [];
        public ICollection<TaskItem> TaskItems { get; set; } = [];
    }
}
