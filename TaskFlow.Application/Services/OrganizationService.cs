using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class OrganizationService(IOrganizationRepository repository, IUserRepository userRepository, ICurrentUserService currentUserService) : 
        EntityService<Organization>(repository), IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<OrganizationDto?> AddWithUserAsync(Organization organization)
        {
            var user = await _userRepository.GetUserByIdAsync(_currentUserService.UserId!);

            organization.Users.Add(user!);
            _repository.Add(organization);
            var success = await _repository.SaveChangesAsync();
            if (!success)
            {
                return null;
            }
            var result = new OrganizationDto
            {
                Id = organization.Id,
                Name = organization.Name
            };
            return result;
        }
        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            organization.Id = id;
            _repository.Attach(organization);
            return await _repository.SaveChangesAsync();
        }
    }
}
