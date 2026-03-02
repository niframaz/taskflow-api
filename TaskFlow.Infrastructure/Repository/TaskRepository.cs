using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class TaskRepository(AppDbContext dbContext) : Repository<TaskItem>(dbContext), ITaskRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public void Attach(TaskItem taskItem)
        {
            _dbContext.Attach(taskItem);
            _dbContext.Entry(taskItem).Property(t => t.Title).IsModified = true;
            _dbContext.Entry(taskItem).Property(t => t.Description).IsModified = true;
        }
    }
}
