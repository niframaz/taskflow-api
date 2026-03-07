using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class OrganizationService(IOrganizationRepository repository, IUserService userService,
        IMembershipService membershipService) : 
        EntityService<Organization>(repository), IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IUserService _userService = userService;
        private readonly IMembershipService _membershipService = membershipService;

        public async Task<bool> AddWithUserAsync(Organization organization)
        {
            var user = await _userService.GetLoggedUserAsync();
            await _membershipService.AddAsync(new Membership
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
        public async Task<IEnumerable<Organization>> GetAllForUserAsync()
        {
            return await _repository.GetAllForUserAsync(_userService.LoggedUserId!);
        }
        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            var memberships = await _membershipService.GetUserOrgRolesAsync(_userService.LoggedUserId!);
            if (!memberships.Any(m => m.Key == id))
            {
                throw new UnauthorizedAccessException("User does not have access to this organization.");
            }
            organization.Id = id;
            _repository.Attach(organization);
            return await _repository.SaveChangesAsync();
        }
    }
}
