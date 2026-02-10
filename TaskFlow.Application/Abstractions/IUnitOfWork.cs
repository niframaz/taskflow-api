namespace TaskFlow.Application.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IUserRepository Users { get; }
        ITaskRepository Tasks { get; }
        IProjectRepository Projects { get; }
        IOrganizationRepository Organizations { get; }
        IMembershipRepository Memberships { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
