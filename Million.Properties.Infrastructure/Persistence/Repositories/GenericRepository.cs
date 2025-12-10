using Microsoft.EntityFrameworkCore;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Domain.Common;

namespace Million.Properties.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(PropertiesDbContext context): IGenericRepository<T> where T : BaseEntityModel
{
    protected readonly PropertiesDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
