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

        }
    }
}
