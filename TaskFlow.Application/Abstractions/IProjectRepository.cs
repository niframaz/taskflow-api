using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IProjectRepository : IRepository<Project>
    {
        void Attach(Project project);
        Task<IEnumerable<Project>> GetAllByOrganizationIdAsync(int organizationId);
        Task<IEnumerable<Project>> GetAllByUserMembershipsAsync(IEnumerable<int> organizationIds);
    }
}
