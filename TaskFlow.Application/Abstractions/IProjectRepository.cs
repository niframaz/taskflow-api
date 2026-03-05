using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IProjectRepository : IRepository<Project>
    {
        void Attach(Project project);
    }
}
