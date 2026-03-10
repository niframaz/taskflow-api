using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
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
                .AsNoTracking()
                .Include(x => x.OrganizationRoles)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
        }
        public async Task<IList<Membership>> GetMembershipsForOrgAsync(int organizationId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.OrganizationId == organizationId)
                .Include(x => x.OrganizationRoles.Where(o => o.Id == organizationId))
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
        public void AddMembershipRoleAsync(Membership membership, OrgRole role)
        {
            var existingRole = membership.OrganizationRoles.FirstOrDefault(r => r.Role == role);

            if (existingRole != null)
            {
                return;
            }
            else
            {
                membership.OrganizationRoles.Add(new OrganizationRole { Role = role });
            }
        }
    }
}
