using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<bool> ValidateUserPasswordAsync(ApplicationUser user, string password);
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string role);
        Task<bool> CreateRoleAsync(string role);
        Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string role);
    }
}
