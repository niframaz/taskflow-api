using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Configuration;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class UserService(
        IUserRepository repository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        ICurrentUserContext currentUserContext,
        IMemoryCache cache,
        IJwtOptions jwtOptions,
        IOptions<CacheSettings> cacheSettings) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserContext _currentUserContext = currentUserContext;
        private readonly IMemoryCache _cache = cache;
        private readonly IJwtOptions _jwtOptions = jwtOptions;
        private readonly CacheSettings _cacheSettings = cacheSettings.Value;

        public string? MyId => _currentUserContext.UserId;

        public async Task<ApplicationUser> GetMeAsync()
        {
            if (string.IsNullOrEmpty(MyId))
                throw new UnauthorizedAccessException("User not authenticated");

            var cacheKey = $"CurrentUser_{MyId}";

            var user = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheSettings.UserCacheExpirationMinutes);
                return await _repository.GetUserByIdAsync(MyId!);
            });

            if (user == null)
                throw new InvalidOperationException($"User {MyId} not found");

            return user;
        }
        public void InvalidateMyCache()
        {
            _cache.Remove($"CurrentUser_{MyId}");
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
        public async Task<AuthResponseDto?> LoginAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user is not null)
            {
                var isValidPassword = await _repository.ValidateUserPasswordAsync(user, password);
                if (isValidPassword)
                {
                    var token = await _jwtService.GenerateToken(user);
                    var result = new AuthResponseDto
                    {
                        Token = token,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                        User = new UserDto
                        {
                            Id = user.Id,
                            Name = user.Name!,
                            Email = user.Email!,
                            Roles = await _repository.GetAllRolesForUserAsync(user)
                        }
                    };
                    return result;
                }
            }
            return null;
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
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            return user;
        }
    }
}
