using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class UserService(IUserRepository repository, IJwtService jwtService, IUnitOfWork unitOfWork, 
        IMembershipService membershipService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMembershipService _membershipService = membershipService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _cache = cache;

        public string? LoggedUserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<ApplicationUser> GetLoggedUserAsync()
        {
            var cacheKey = $"CurrentUser_{LoggedUserId}";

            return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);//add to appsettings

                var user = await _repository.GetUserByIdAsync(LoggedUserId!);

                return user;
            }))!;
        }
        public void InvalidateLoggedUserCache()
        {
            _cache.Remove($"CurrentUser_{LoggedUserId}");
        }
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
            var user = await _repository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var isValidPassword = await _repository.ValidateUserPasswordAsync(user, password);
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

            var currentUserMembership = await _membershipService.GetUserOrgRolesAsync(LoggedUserId!);

            var user = await _repository.GetUserByEmailAsync(email);

            if (user is not null)
            {
                //var userOrg = user.Organizations.Select(o => o.Id);

                var result = await _repository.AddUserToRoleAsync(user, role.ToString());
                return result;
            }
            return false;
        }

        public async Task<bool> RemoveUserRoleAsync(string email, UserRole role)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var result = await _repository.RemoveRoleFromUserAsync(user, role.ToString());
                return result;
            }
            return false;
        }
        public async Task<bool> CreateRoleAsync(UserRole role)
        {
            var result = await _repository.CreateRoleAsync(role.ToString());
            return result;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            return user!;
        }
    }
}
