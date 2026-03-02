using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class EntityService<T>(IRepository<T> repository) : IEntityService<T> where T : class
    {
        private readonly IRepository<T> _repository = repository;
        public async Task<bool> AddAsync(T entity)
        {
            _repository.Add(entity);
            var result = await _repository.SaveChangesAsync();
            return result;
        }
        public async Task<bool> RemoveAsync(int id) 
        {
            var entity = await _repository.GetAsync(id);
            if (entity != null)
            {
                _repository.Remove(entity);
                var result = await _repository.SaveChangesAsync();
                return result;
            }
            throw new KeyNotFoundException();
        }
        public async Task<T?> GetAsync(int id) 
        { 
            return await _repository.GetAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync() 
        { 
            return await _repository.GetAllAsync();
        }
    }
}
