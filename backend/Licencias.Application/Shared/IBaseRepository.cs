using System.Linq.Expressions;

namespace Licencias.Application.Shared
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, IEnumerable<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool disableTracking = true);
        Task<T?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
