using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class OrganizationMembershipRepository(AppDbContext dbContext) : Repository<OrganizationMembership>(dbContext), IOrganizationMembershipRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<List<OrganizationMembership>> GetUserMembershipsAsync(string userId)
        {
            return await _dbContext.OrganizationMemberships
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.OrganizationRoles)
                .ToListAsync();
        }
    }
}
