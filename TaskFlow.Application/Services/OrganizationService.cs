using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class OrganizationService(IOrganizationRepository repository) : EntityService<Organization>(repository), IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            organization.Id = id;
            _repository.Attach(organization);
            return await _repository.SaveChangesAsync();
        }
    }
}
