using Microsoft.Extensions.Caching.Memory;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class MembershipService(IMemoryCache cache, IMembershipRepository repository, IUserService userService) : EntityService<Membership>(repository), IMembershipService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IMembershipRepository _repository = repository;
        private readonly IUserService _userService = userService;

        public async Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string? userId = null)
        {
            userId ??= _userService.LoggedUserId;

            var cacheKey = $"UserOrgRoles_{userId}";

            return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);//add to appsettings

                var memberships = await _repository.GetUserMembershipsAsync(userId!);

                return memberships.ToDictionary(
                    m => m.OrganizationId,
                    m => m.OrganizationRoles.Select(r => r.Role.ToString()).ToList()
                );
            }))!;
        }

        public void InvalidateUserOrgRolesCache(string userId)
        {
            _cache.Remove($"UserOrgRoles_{userId}");
        }

        public async Task<List<Membership>> GetUserMembershipsAsync(string? userId = null)
        {
            var result = await _repository.GetUserMembershipsAsync(userId ?? _userService.LoggedUserId!);
            return result;
        }
    }
}
