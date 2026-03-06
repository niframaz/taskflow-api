namespace TaskFlow.Domain.Entities
{
    public class OrganizationUserRole
    {
        public int Id { get; set; }
        public ICollection<OrganizationRole> OrganizationRoles { get; set; } = default!;
        public int OrganizationId { get; set; }
        public required Organization Organization { get; set; }
        public required ApplicationUser User { get; set; }
        public string UserId { get; set; } = default!;
    }
}
