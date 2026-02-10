using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationService : IEntityService<Organization>
    {
        Task<Result<OrganizationDto>> CreateOrganizationAsync(string name, string? description);
        Task<Result<OrganizationDto>> UpdateOrganizationAsync(int id, string name, string? description);
        Task<Result> DeleteOrganizationAsync(int id);
        Task<Result<OrganizationDto>> GetOrganizationByIdAsync(int id);
        Task<Result<IEnumerable<OrganizationSummaryDto>>> GetMyOrganizationsAsync();
    }
}
