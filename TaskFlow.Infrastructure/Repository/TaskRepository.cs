using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class TaskRepository(AppDbContext context) : Repository<TaskItem>(context), ITaskRepository
    {
        public void Attach(TaskItem taskItem)
        {
            var entry = _dbSet.Entry(taskItem);

            entry.Property(o => o.Title).IsModified = true;
            entry.Property(o => o.Description).IsModified = true;
            entry.Property(o => o.UserId).IsModified = true;
        }

        public override async Task<TaskItem?> GetAsync(int id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.Organization)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.Organization)
                .Where(t => t.ProjectId == projectId)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
