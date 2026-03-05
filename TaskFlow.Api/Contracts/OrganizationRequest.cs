using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Contracts
{
    public class OrganizationRequest
    {
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
