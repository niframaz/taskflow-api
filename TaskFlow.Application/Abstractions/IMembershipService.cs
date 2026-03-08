using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipService : IEntityService<Membership>
    {
        Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string? userId = null);
        void InvalidateUserOrgRolesCache(string userId);
        Task<List<Membership>> GetUserMembershipsAsync(string? userId = null);
        Task<bool> LoggedUserIsAdminAndHasAccessToOrgAsync(int id);
    }
}
