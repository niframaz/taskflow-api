using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Infrastructure.Context
{
    public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
