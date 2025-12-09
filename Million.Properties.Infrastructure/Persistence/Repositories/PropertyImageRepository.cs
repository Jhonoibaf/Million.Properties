using Microsoft.EntityFrameworkCore;
using Million.Properties.Application.Contracts.Persistence;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence.Repositories;

public class PropertyImageRepository (PropertiesDbContext context ): GenericRepository<PropertyImage>(context), IPropertyImageRepository
{


    public async Task AddAsync(PropertyImage image)
    {
        await context.PropertyImages.AddAsync(image);
    }

    public async Task<Dictionary<int, List<PropertyImage>>> GetAllImagesByPropertyIdsAsync(IEnumerable<int> propertyIds)
    {
        var ids = propertyIds.Distinct().ToList();

        if (ids.Count == 0)
            return new Dictionary<int, List<PropertyImage>>();

        return await context.PropertyImages
            .Where(pi => ids.Contains(pi.IdProperty))
            .OrderBy(pi => pi.IdProperty)
            .ThenBy(pi => pi.IdPropertyImage)
            .GroupBy(pi => pi.IdProperty)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Where(x => x.Enabled && !string.IsNullOrEmpty(x.File)).ToList()
            );
    }

    public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId)
    {
        return await context.PropertyImages
            .Where(pi => pi.IdProperty == propertyId)
            .ToListAsync();
    }
}
