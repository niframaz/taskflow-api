//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using TaskFlow.Infrastructure.Data;

//namespace TaskFlow.Infrastructure.Repository
//{
//    public class OrganizationUserRoleRepository(IMemoryCache cache, AppDbContext dbContext)
//    {
//        private readonly IMemoryCache _cache = cache;
//        private readonly AppDbContext _dbContext = dbContext;
//        public async Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string userId)
//        {
//            var cacheKey = $"UserOrgRoles_{userId}";

//            if (!_cache.TryGetValue(cacheKey, out Dictionary<int, List<string>> orgRoles))
//            {
//                orgRoles = await _dbContext.OrganizationUserRoles
//                    .Where(u => u.UserId == userId)
//                    .Include(u => u.OrgRoles)
//                    .ToDictionaryAsync(
//                        u => u.OrganizationId,
//                        u => u.OrgRoles.Select(r => nameof(r)).ToList()
//                    );
//                _cache.Set(cacheKey, orgRoles, TimeSpan.FromMinutes(5));
//            }
//            return orgRoles;
//        }

//        public void InvalidateCache(string userId)
//        {
//            _cache.Remove($"UserOrgRoles_{userId}");
//        }
//    }
//}
