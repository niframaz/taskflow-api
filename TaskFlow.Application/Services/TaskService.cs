using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class TaskService(ITaskRepository repository) : EntityService<TaskItem>(repository), ITaskService
    {
        private readonly ITaskRepository _repository = repository;
        public async Task<bool> UpdateAsync(int id, TaskItem taskItem)
        {
            taskItem.Id = id;
            _repository.Attach(taskItem);

            return await _repository.SaveChangesAsync();
        }
    }
}
