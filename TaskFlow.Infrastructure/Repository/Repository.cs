using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public abstract class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
