using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Repository
{
    public class UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<bool> ValidateUserPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<bool> AddUserToRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
        public async Task<bool> CreateRoleAsync(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            return result.Succeeded;
        }
        public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user ,string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }
        public async Task<IList<string>> GetAllRolesForUserAsync(ApplicationUser user)
        {
            var result = await _userManager.GetRolesAsync(user);
            return result;
        }
    }
}
