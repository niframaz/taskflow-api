using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class ProjectService(IProjectRepository repository) : EntityService<Project>(repository), IProjectService
    {
        private readonly IProjectRepository _repository = repository;
        public async Task<bool> UpdateAsync(int id, Project project)
        {
            project.Id = id;
            _repository.Attach(project);

            return await _repository.SaveChangesAsync();
        }
    }
}
