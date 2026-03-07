using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationService : IEntityService<Organization>
    {
        Task<bool> UpdateAsync(int id, Organization organization);
        Task<bool> AddWithUserAsync(Organization organization);
    }
}
