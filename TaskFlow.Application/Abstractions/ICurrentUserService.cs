namespace TaskFlow.Application.Abstractions
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserRole { get; } 
    }
}
