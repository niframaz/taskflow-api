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

        public async override Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _repository.GetAllForUserAsync(_userService.LoggedUserId!);
        }
        public async override Task<Organization?> GetAsync(int id)
        {
            return await _repository.GetForUserAsync(_userService.LoggedUserId!, id);
        }
        public async override Task<bool> AddAsync(Organization organization)
        {
            var user = await _userService.GetLoggedUserAsync();
            var success = await _membershipService.AddAsync(new Membership
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
            _userService.InvalidateLoggedUserCache();
            return success;
        }

        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            if (await _membershipService.LoggedUserIsAdminAndHasAccessToOrgAsync(id))
            {
                organization.Id = id;
                _repository.Attach(organization);
                return await _repository.SaveChangesAsync();
            }
            throw new UnauthorizedAccessException("User does not have access to this organization.");
        }
        public async override Task<bool> RemoveAsync(int id)
        {
            if (await _membershipService.LoggedUserIsAdminAndHasAccessToOrgAsync(id))
            {
                return await base.RemoveAsync(id);                
            }
            throw new UnauthorizedAccessException("User does not have access to this organization.");
        }
    }
}
