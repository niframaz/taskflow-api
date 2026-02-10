using Microsoft.EntityFrameworkCore.Storage;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        public UnitOfWork(
            AppDbContext context,
            IUserRepository users,
            ITaskRepository tasks,
            IProjectRepository projects,
            IOrganizationRepository organizations,
            IMembershipRepository memberships)
        {
            _context = context;
            Users = users;
            Tasks = tasks;
            Projects = projects;
            Organizations = organizations;
            Memberships = memberships;
        }

        public IUserRepository Users { get; }
        public ITaskRepository Tasks { get; }
        public IProjectRepository Projects { get; }
        public IOrganizationRepository Organizations { get; }
        public IMembershipRepository Memberships { get; }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null) await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null) await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null) await _transaction.DisposeAsync();
            await _context.DisposeAsync();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
