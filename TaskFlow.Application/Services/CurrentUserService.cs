using Microsoft.AspNetCore.Http;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
         private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
         public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
         public string? OrganizationId => _httpContextAccessor.HttpContext?.User.FindFirst("OrganizationId")?.Value;

    }
}
