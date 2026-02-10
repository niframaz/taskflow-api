using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class ProjectRepository(AppDbContext context) : Repository<Project>(context), IProjectRepository
    {
        public void Attach(Project project)
        {
            var entry = _dbSet.Entry(project);

            entry.Property(o => o.Name).IsModified = true;
            entry.Property(o => o.Description).IsModified = true;
        }
        public async Task<IEnumerable<Project>> GetAllByOrganizationIdAsync(int organizationId)
        {
            return await _dbSet
                .Include(p => p.Organization)
                .Include(p => p.TaskItems)
                    .ThenInclude(t => t.User)
                .Where(p => p.OrganizationId == organizationId)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllByUserMembershipsAsync(IEnumerable<int> organizationIds)
        {
            return await _dbSet
                .Include(p => p.Organization)
                .Include(p => p.TaskItems)
                    .ThenInclude(t => t.User)
                .Where(p => organizationIds.Contains(p.OrganizationId))
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
