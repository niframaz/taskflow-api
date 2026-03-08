using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class OrganizationRepository(AppDbContext context) : Repository<Organization>(context), IOrganizationRepository
    {
        public void Attach(Organization organization)
        {
            var entry = _dbSet.Entry(organization);

            entry.Property(o => o.Name).IsModified = true;
            entry.Property(o => o.Description).IsModified = true;
        }
        public async Task<IEnumerable<Organization>> GetAllAsync(string userId)
        {
            return await _dbSet.Where(o => o.Memberships.Any(m => m.UserId == userId)).ToListAsync();
        }
        public async Task<Organization?> GetAsync(int id, string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.Id == id && o.Memberships.Any(m => m.UserId == userId));
        }
    }
}
