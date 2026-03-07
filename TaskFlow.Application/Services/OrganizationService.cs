using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class OrganizationService(IOrganizationRepository repository, IUserRepository userRepository, ICurrentUserService currentUserService,
        IOrganizationMembershipRepository membershipRepository) : 
        EntityService<Organization>(repository), IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IOrganizationMembershipRepository _membershipRepository = membershipRepository;

        public async Task<bool> AddWithUserAsync(Organization organization)
        {
            var user = await _userRepository.GetUserByIdAsync(_currentUserService.UserId!);
            _membershipRepository.Add(new OrganizationMembership
            { 
                Organization = organization, 
                User = user!,
                OrganizationRoles =
                [
                    new() {
                        Role = OrgRole.Admin
                    }
                ]
            });
            var success = await _repository.SaveChangesAsync();
            return success;
        }
        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            organization.Id = id;
            _repository.Attach(organization);
            return await _repository.SaveChangesAsync();
        }
    }
}
