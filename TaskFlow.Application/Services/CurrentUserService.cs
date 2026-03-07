using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
         private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
         public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
