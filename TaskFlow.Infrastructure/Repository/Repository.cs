using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public abstract class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual async Task<bool> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
