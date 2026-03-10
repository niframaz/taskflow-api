using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task<List<Membership>> GetUserMembershipsAsync(string userId);
        Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string userId);
        Task<IList<Membership>> GetMembershipsForOrgAsync(int organizationId);
    }
}
