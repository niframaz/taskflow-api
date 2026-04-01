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
        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            var userMemberships = await _membershipService.GetUserMembershipsAsync();
            var accessibleOrgIds = userMemberships.Select(m => m.OrganizationId);
            var projects = await _repository.GetAllByUserMembershipsAsync(accessibleOrgIds);
            return projects;
        }
        public override async Task<bool> AddAsync(Project entity)
        {
            var iAmAnAdminOfOrg = await _membershipService.IAmAdminOfOrgAsync(entity.OrganizationId);
            if (!iAmAnAdminOfOrg)
                throw new UnauthorizedAccessException("User does not have access to delete this project.");
            _repository.Add(entity);
            var result = await _repository.SaveChangesAsync();
            return result;
        }
        public override async Task<bool> RemoveAsync(int id)
        {
            var project = await _repository.GetAsync(id) ?? throw new KeyNotFoundException("Project not found.");
            var iAmAnAdminOfOrg = await _membershipService.IAmAdminOfOrgAsync(project.OrganizationId);
            if (!iAmAnAdminOfOrg)
                throw new UnauthorizedAccessException("User does not have access to delete this project.");
            _repository.Remove(project);
            var result = await _repository.SaveChangesAsync();
            return result;
        }
        public async Task<IEnumerable<Project>> GetAllForOrgAsync(int id)
        {
            _ = _membershipService.GetUserMembershipForOrgAsync(id)
                ?? throw new UnauthorizedAccessException("User does not have access to this organization.");
            return await _repository.GetAllByOrganizationIdAsync(id);
        }
    }
}
