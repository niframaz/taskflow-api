using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationMembershipRepository
    {
        Task<List<OrganizationMembership>> GetUserMembershipsAsync(string userId);
    }
}
