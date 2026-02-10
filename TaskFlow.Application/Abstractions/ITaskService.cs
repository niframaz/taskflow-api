using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface ITaskService : IEntityService<TaskItem>
    {
        Task<Result<TaskItemDto>> CreateTaskAsync(int projectId, string title, string? description);
        Task<Result<TaskItemDto>> UpdateTaskAsync(int id, string title, string? description);
        Task<Result> DeleteTaskAsync(int id);
        Task<Result<TaskItemDto>> GetTaskByIdAsync(int id);
        Task<Result<IEnumerable<TaskItemSummaryDto>>> GetTasksByProjectAsync(int projectId);
        Task<Result<TaskItemDto>> AssignTaskToUserAsync(int taskId, string userId);
        Task<Result<TaskItemDto>> UnassignTaskAsync(int taskId);
    }
}
