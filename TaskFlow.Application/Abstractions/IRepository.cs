namespace TaskFlow.Application.Abstractions
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void Remove(T entity);
        Task<bool> SaveChangesAsync();
    }
}
