using Microsoft.Extensions.Caching.Memory;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class MembershipService(IMemoryCache cache, IMembershipRepository repository, IUserService userService) : EntityService<Membership>(repository), 
        IMembershipService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IMembershipRepository _repository = repository;
        private readonly IUserService _userService = userService;

        private string GetMembershipCacheKey(string userId) => $"Membership_{userId}";
        public async Task<List<Membership>> GetUserMembershipsAsync(string? userId = null)
        {
            userId ??= _userService.LoggedUserId;
            var cacheKey = GetMembershipCacheKey(userId!);
            return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);//add to appsettings
                var result = await _repository.GetUserMembershipsAsync(userId!);
                return result;
            }))!;
        }
        public void InvalidateMembership(string? userId = null)
        {
            userId ??= _userService.LoggedUserId;
            _cache.Remove(GetMembershipCacheKey(userId!));
        }
        public async Task<bool> LoggedUserIsAdminAndHasAccessToOrgAsync(int id)
        {
            var memberships = await GetUserMembershipsAsync();
            return memberships.Any(m => m.OrganizationId == id && m.OrganizationRoles.Any(x => x.Role == OrgRole.Admin));
        }
    }
}
