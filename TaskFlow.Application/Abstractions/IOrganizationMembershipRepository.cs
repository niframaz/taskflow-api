using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationMembershipRepository : IRepository<OrganizationMembership>
    {
        Task<List<OrganizationMembership>> GetUserMembershipsAsync(string userId);
    }
}
