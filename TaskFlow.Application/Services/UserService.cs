using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class UserService(IUserRepository userRepository, IJwtService jwtService) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtService _jwtService = jwtService;
        public async Task<string?> RegisterAsync(ApplicationUser user, string password)
        {
            user.UserName = user.Email;
            var result = await _userRepository.CreateUserAsync(user, password);
            if (result)
            {
                await _userRepository.AddUserToRoleAsync(user, UserRole.User.ToString());
                var token = await _jwtService.GenerateToken(user);
                return token;
            }
            return null;
        }
        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var isValidPassword = await _userRepository.ValidateUserPasswordAsync(user, password);
                if (isValidPassword)
                {
                    var token = await _jwtService.GenerateToken(user);
                    return token;
                }
            }
            return null;
        }
        public async Task<bool> AddUserRoleAsync(string email, UserRole role)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userRepository.AddUserToRoleAsync(user, role.ToString());
                return result;
            }
            return false;
        }
        public async Task<bool> CreateRoleAsync(UserRole role)
        {
            var result = await _userRepository.CreateRoleAsync(role.ToString());
            return result;
        }
        public async Task<bool> RemoveUserRoleAsync(string email, UserRole role)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userRepository.RemoveRoleFromUserAsync(user, role.ToString());
                return result;
            }
            return false;
        }
    }
}
