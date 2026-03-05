using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IProjectService : IEntityService<Project>
    {
        Task<bool> UpdateAsync(int id, Project project);
    }
}
