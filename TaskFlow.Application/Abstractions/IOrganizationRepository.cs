using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        void Attach(Organization organization);
        Task<Organization?> GetAsync(int id, string userId);
        Task<IEnumerable<Organization>> GetAllAsync(string userId);
    }
}
