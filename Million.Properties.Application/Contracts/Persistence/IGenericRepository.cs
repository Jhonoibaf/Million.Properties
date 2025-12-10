
using Million.Properties.Domain.Common;

namespace Million.Properties.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntityModel
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync( T entity);
    Task DeleteAsync(int id);
}
