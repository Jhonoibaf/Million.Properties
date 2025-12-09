using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Contracts.Persistence
{
    public interface IPropertyRepository
    {
        Task<Property?> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
        Task AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(string id);
    }
}
