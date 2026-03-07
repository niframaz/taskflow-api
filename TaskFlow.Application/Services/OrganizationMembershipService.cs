using Microsoft.Extensions.Caching.Memory;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Services
{
    public class OrganizationMembershipService(IMemoryCache cache, IOrganizationMembershipRepository repository) : IOrganizationMembershipService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IOrganizationMembershipRepository _repository = repository;
        public async Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string userId)
        {
            var cacheKey = $"UserOrgRoles_{userId}";

            return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);//add to appsettings

                var memberships = await _repository.GetUserMembershipsAsync(userId);

                return memberships.ToDictionary(
                    m => m.OrganizationId,
                    m => m.OrganizationRoles.Select(r => r.Role.ToString()).ToList()
                );
            }))!;
        }

        public void InvalidateCache(string userId)
        {
            _cache.Remove($"UserOrgRoles_{userId}");
        }
    }
}
