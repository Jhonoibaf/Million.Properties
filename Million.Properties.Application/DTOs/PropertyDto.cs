using Million.Properties.Domain.Entities;

namespace Million.Properties.Application.DTOs;

public class PropertyDto
{
    public int IdProperty { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string InternalCode { get; set; } = string.Empty;
    public int Year { get; set; }
    public int? IdOwner { get; set; }
    public ICollection<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
}
