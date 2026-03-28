using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class ProjectService(IProjectRepository repository, IMembershipService membershipService) : EntityService<Project>(repository), IProjectService
    {
        private readonly IProjectRepository _repository = repository;
        private readonly IMembershipService _membershipService = membershipService;
        public async Task<bool> UpdateAsync(int id, Project project)
        {
            project.Id = id;
            _repository.Attach(project);

            return await _repository.SaveChangesAsync();
        }
        public override async Task<Project?> GetAsync(int id)
        {
            var project = await _repository.GetAsync(id);
            if (project != null && await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
                    throw new UnauthorizedAccessException("User does not have access to this project.");
            return project;
        }
    }
}
