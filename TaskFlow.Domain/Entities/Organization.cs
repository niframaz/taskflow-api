using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public ICollection<Project> Projects { get; set; } = [];
        //public ICollection<ApplicationUser> Users { get; set; } = [];
        public ICollection<OrganizationMembership> OrganizationMemberships { get; set; } = [];
    }
}
