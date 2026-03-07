using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IMembershipService : IEntityService<Membership>
    {
        Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string userId);
        void InvalidateCache(string userId);
    }
}
