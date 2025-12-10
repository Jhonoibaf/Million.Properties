using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Contracts.Persistence;

public interface IPropertyImageRepository : IGenericRepository<PropertyImage>
{
    Task<Dictionary<int, List<PropertyImage>>> GetAllImagesByPropertyIdsAsync(IEnumerable<int> propertyIds);
    Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId);
}
