using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipService : IEntityService<Membership>
    {
        void InvalidateMembership(string? userId = null);
        Task<List<Membership>> GetUserMembershipsAsync(string? userId = null);
        Task<bool> IAmAdminAndHasAccessToOrgAsync(int id);
        Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string? userId = null);
        Task<IEnumerable<Membership>> GetAllMembershipsForMyOrgAsync(int orgId);
        Task<bool> AddMembershipRoleAsync(int orgId, string userId, OrgRole role);
    }
}
