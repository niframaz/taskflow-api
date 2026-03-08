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
                .ToListAsync();
        }
    }
}
