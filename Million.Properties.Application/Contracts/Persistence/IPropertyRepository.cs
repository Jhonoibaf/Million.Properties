using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.Contracts.Persistence
{
    public interface IPropertyRepository: IGenericRepository<Property>
    {
        Task<IEnumerable<Property>> GetAllWithFiltersAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
    }
}
