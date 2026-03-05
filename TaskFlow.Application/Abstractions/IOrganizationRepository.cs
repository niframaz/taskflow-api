using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        void Attach(Organization organization);
    }
}
