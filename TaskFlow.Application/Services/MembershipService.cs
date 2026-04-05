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
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);//add to appsettings
                var result = await _repository.GetUserMembershipsAsync(userId!);
                return result;
            }))!;
        }
        public void InvalidateMembership(string? userId = null)
        {
            userId ??= _userService.MyId;
            _cache.Remove(GetMembershipCacheKey(userId!));
        }
        public async Task<bool> IAmAdminOfOrgAsync(int id)
        {
            var memberships = await GetUserMembershipsAsync();
            return memberships.Any(m => m.OrganizationId == id && m.OrganizationRoles.Any(x => x.Role == OrgRole.Admin));
        }
        public async Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string? userId = null)
        {
            var memberships = await GetUserMembershipsAsync(userId);
            return memberships.FirstOrDefault(m => m.OrganizationId == organizationId);
        }
        public async Task<IEnumerable<Membership>> GetAllMembershipsForMyOrgAsync(int orgId)
        {
            var memberships = await _repository.GetOrganizationMembershipsAsync(orgId);
            var iAmAMember = memberships.Any(m => m.UserId == _userService.MyId);
            return iAmAMember
                ? memberships
                : throw new UnauthorizedAccessException("User does not have access to this organization.");
        }
        public async Task<bool> AddMembershipRoleAsync(int orgId, string userId, OrgRole role)
        {
            if(await IAmAdminOfOrgAsync(orgId))
                throw new UnauthorizedAccessException("User does not have access to edit this member.");
            var member = await GetUserMembershipForOrgAsync(orgId, userId) 
                ?? throw new InvalidOperationException("User is not a member of this organization.");
            _repository.AddMembershipRoleAsync(member, role);
            InvalidateMembership(member.UserId);
            return await _repository.SaveChangesAsync();
        }
    }
}
