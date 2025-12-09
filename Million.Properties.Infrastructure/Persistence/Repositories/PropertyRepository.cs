using Microsoft.EntityFrameworkCore;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Domain.Entities;


namespace Million.Properties.Infrastructure.Persistence.Repositories;

public class PropertyRepository(PropertiesDbContext context) : GenericRepository<PropertyRepository>(context), IPropertyRepository
{
    public async Task<Property?> GetByIdAsync(int id) =>
        await context.Properties.Where(x => x.IdProperty == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var query = context.Properties.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(address))
            query = query.Where(x => EF.Functions.Like(x.Address, $"%{address}%"));

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        return await query.ToListAsync();
    }

    public async Task AddAsync(Property property) =>
        await context.Properties.AddAsync(property);

    public async Task UpdateAsync(Property property)
    {
        context.Properties.Update(property);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id) =>
        await context.Properties.Where(x => x.IdProperty.ToString() == id).ExecuteDeleteAsync();
}
