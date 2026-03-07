using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class UserService(IUserRepository userRepository, IJwtService jwtService, IUnitOfWork unitOfWork, ICurrentUserService currentUser, 
        IOrganizationMembershipService membershipService) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly IOrganizationMembershipService _membershipService = membershipService;
        public async Task<string?> RegisterAsync(ApplicationUser user, string password)
        {
            user.UserName = user.Email;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Users.CreateUserAsync(user, password);
                if (!result)
                {
                    await _unitOfWork.RollbackAsync();
                    return null;
                }
                await _unitOfWork.Users.AddUserToRoleAsync(user, nameof(UserRole.User));
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            var token = await _jwtService.GenerateToken(user);
            return token;
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
            //var currentUserRole = int.Parse(_currentUser.UserRole!);
            //if (currentUserRole != (int)UserRole.Admin)
            //    throw new UnauthorizedAccessException("Only admin in the organization can assign roles");

            var currentUserMembership = await _membershipService.GetUserOrgRolesAsync(_currentUser.UserId!);

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user is not null)
            {
                //var userOrg = user.Organizations.Select(o => o.Id);

                var result = await _userRepository.AddUserToRoleAsync(user, role.ToString());
                return result;
            }
            return false;
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
        public async Task<bool> CreateRoleAsync(UserRole role)
        {
            var result = await _userRepository.CreateRoleAsync(role.ToString());
            return result;
        }
    }
}
