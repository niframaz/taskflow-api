using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface ITaskService : IEntityService<TaskItem>
    {

        Task<bool> UpdateAsync(int id, TaskItem taskItem);
    }
}
