namespace TaskFlow.Application.Abstractions
{
    public interface IEntityService<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<bool> RemoveAsync(int id);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
