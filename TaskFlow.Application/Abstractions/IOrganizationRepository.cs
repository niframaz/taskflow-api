using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        void Attach(Organization organization);
        Task<IEnumerable<Organization>> GetAllForUserAsync(string userId);
        Task<Organization?> GetForUserAsync(string userId, int id);
    }
}
