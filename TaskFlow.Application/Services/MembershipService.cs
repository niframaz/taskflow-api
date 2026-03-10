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

        private static string GetMembershipCacheKey(string userId)
        {
            return $"Membership_{userId}";
        }
        public async Task<List<Membership>> GetUserMembershipsAsync(string? userId = null)
        {
            userId ??= _userService.MyId;
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
            userId ??= _userService.MyId;
            _cache.Remove(GetMembershipCacheKey(userId!));
        }
        public async Task<bool> IAmAdminAndHasAccessToOrgAsync(int id)
        {
            var memberships = await GetUserMembershipsAsync();
            return memberships.Any(m => m.OrganizationId == id && m.OrganizationRoles.Any(x => x.Role == OrgRole.Admin));
        }
        public async Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string? userId = null)
        {
            userId ??= _userService.MyId;
            return await _repository.GetUserMembershipForOrgAsync(organizationId, userId!);
        }
        public async Task<IList<Membership>> GetAllMembershipsForMyOrgAsync(int orgId)
        {
            var myMembership = await GetUserMembershipForOrgAsync(orgId);
            return myMembership is null
                ? throw new UnauthorizedAccessException("User does not have access to this organization.")
                : await _repository.GetMembershipsForOrgAsync(orgId);
        }
        public async Task<bool> AddMembershipRoleAsync(int orgId, string userId, OrgRole role)
        {
            var hasAccess = await IAmAdminAndHasAccessToOrgAsync(orgId);
            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("User does not have access to edit this member.");
            }
            var member = await _repository.GetUserMembershipForOrgAsync(orgId, userId);
            if(member is null)
            {
                return false;
            }
            _repository.AddMembershipRoleAsync(member, role);
            InvalidateMembership(member.UserId);
            return await _repository.SaveChangesAsync();
        }
    }
}
