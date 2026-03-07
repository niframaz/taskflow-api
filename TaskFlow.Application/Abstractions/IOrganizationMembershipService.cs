namespace TaskFlow.Application.Abstractions
{
    public interface IOrganizationMembershipService
    {
        Task<Dictionary<int, List<string>>> GetUserOrgRolesAsync(string userId);
        void InvalidateCache(string userId);
    }
}
