using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task<List<Membership>> GetUserMembershipsAsync(string userId);
        Task<Membership?> GetUserMembershipForOrgByEmailAsync(int organizationId, string email);
        Task<List<Membership>> GetOrganizationMembershipsAsync(int orgId);
    }
}
