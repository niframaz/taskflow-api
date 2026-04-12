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
        public async Task<bool> AddMembershipAsync(int orgId, string userId, OrgRole role)
        {
            var myMembership = await GetUserMembershipForOrgAsync(orgId);

            if (myMembership is not null && myMembership.OrganizationRoles.Any(r => r.Role == OrgRole.Admin))
            {
                var member = await GetUserMembershipForOrgAsync(orgId, userId) ?? new Membership
                {
                    Organization = myMembership.Organization,
                    User = await _userService.GetUserByIdAsync(userId) ?? throw new InvalidOperationException("User not found."),
                };
                AddMembershipRole(member, role);
                _repository.Add(member);
                InvalidateMembership(userId);
                return await _repository.SaveChangesAsync();
            }
            throw new UnauthorizedAccessException("You are not an admin of this organization.");
        }
        private static void AddMembershipRole(Membership membership, OrgRole role)
        {
            //add costraint: prevent duplicate roles for the same membership
            var existingRole = membership.OrganizationRoles.FirstOrDefault(r => r.Role == role);

            if (existingRole != null)
            {
                throw new InvalidOperationException("Role already exist for the user");
            }
            else
            {
                membership.OrganizationRoles.Add(new OrganizationRole { Role = role });
            }
        }
    }
}
