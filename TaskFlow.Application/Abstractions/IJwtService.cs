using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Abstractions
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
