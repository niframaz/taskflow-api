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
                .Where(x => x.UserId == userId)
                .Include(x => x.OrganizationRoles)
                .Include(x => x.Organization)
                .ToListAsync();
        }
        public async Task<List<Membership>> GetOrganizationMembershipsAsync(int orgId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.OrganizationId == orgId)
                .Include(x => x.OrganizationRoles)
                .Include(x => x.User)
                .ToListAsync();
        }
        //remove
        public async Task<Membership?> GetUserMembershipForOrgByEmailAsync(int organizationId, string email)
        {
            return await _dbSet
                .Include(x => x.OrganizationRoles)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.OrganizationId == organizationId && x.User.Email == email);
        }
    }
}
