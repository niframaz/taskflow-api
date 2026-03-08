using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipService : IEntityService<Membership>
    {
        void InvalidateMembership(string? userId = null);
        Task<List<Membership>> GetUserMembershipsAsync(string? userId = null);
        Task<bool> LoggedUserIsAdminAndHasAccessToOrgAsync(int id);
    }
}
