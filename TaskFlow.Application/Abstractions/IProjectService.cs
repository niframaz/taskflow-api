using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IProjectService : IEntityService<Project>
    {
        Task<Result<ProjectDto>> CreateProjectAsync(int organizationId, string name, string? description);
        Task<Result<ProjectDto>> UpdateProjectAsync(int id, string name, string? description);
        Task<Result> DeleteProjectAsync(int id);
        Task<Result<ProjectDto>> GetProjectByIdAsync(int id);
        Task<Result<IEnumerable<ProjectSummaryDto>>> GetProjectsByOrganizationAsync(int organizationId);
        Task<Result<IEnumerable<ProjectSummaryDto>>> GetMyProjectsAsync();
    }
}
