using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        void Attach(TaskItem taskItem);
    }
}
