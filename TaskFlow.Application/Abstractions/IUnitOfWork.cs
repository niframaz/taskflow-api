namespace TaskFlow.Application.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IUserRepository Users { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
