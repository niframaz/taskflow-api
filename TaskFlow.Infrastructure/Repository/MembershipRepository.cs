using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class MembershipRepository(AppDbContext context) : Repository<Membership>(context), IMembershipRepository
    {
        public async Task<List<Membership>> GetUserMembershipsAsync(string userId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.OrganizationRoles)
                .Include(x => x.User)
                .ToListAsync();
        }
        public async Task<Membership?> GetUserMembershipForOrgAsync(int organizationId, string userId)
        {
            return await _dbSet
                .Include(x => x.OrganizationRoles)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
        }
    }
}
