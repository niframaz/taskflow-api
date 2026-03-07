using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class MembershipRepository(AppDbContext dbContext) : Repository<Membership>(dbContext), IMembershipRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<List<Membership>> GetUserMembershipsAsync(string userId)
        {
            return await _dbContext.Memberships
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.OrganizationRoles)
                .ToListAsync();
        }
    }
}
