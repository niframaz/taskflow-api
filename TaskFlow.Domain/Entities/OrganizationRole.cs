using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class OrganizationRole
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        public int OrganizationId { get; set; }
        public required Organization Organization { get; set; }
        public string UserId { get; set; } = default!;
        public required ApplicationUser User { get; set; }
    }
}
