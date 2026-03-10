using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task<List<Membership>> GetUserMembershipsAsync(string userId);
        Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string userId);
        Task<IList<Membership>> GetMembershipsForOrgAsync(int organizationId);
        void AddMembershipAsync(Membership membership, OrgRole role);
        Task<Membership?> GetUserMembershipForOrgByEmailAsync(int organizationId, string email);
    }
}
