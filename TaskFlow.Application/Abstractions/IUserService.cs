using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Abstractions
{
    public interface IUserService
    {
        public string? MyId { get; }
        Task<string?> RegisterAsync(ApplicationUser user, string password);
        Task<string?> LoginAsync(string email, string password);
        Task<bool> CreateRoleAsync(UserRole role);
        Task<bool> RemoveUserRoleAsync(string email, UserRole role);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetMeAsync();
        void InvalidateMyCache();

    }
}
