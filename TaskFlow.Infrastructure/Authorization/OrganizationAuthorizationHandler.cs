using Microsoft.AspNetCore.Authorization;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Authorization
{
    public class OrganizationAuthorizationHandler :
        AuthorizationHandler<OrganizationAdminRequirement, int>
    {
        private readonly IMembershipService _membershipService;

        public OrganizationAuthorizationHandler(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OrganizationAdminRequirement requirement,
            int organizationId)
        {
            if (await _membershipService.IAmAdminOfOrgAsync(organizationId))
            {
                context.Succeed(requirement);
            }
        }
    }

    public class OrganizationAdminRequirement : IAuthorizationRequirement { }
}
