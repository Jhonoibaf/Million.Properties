using Million.Properties.Domain.Common;

namespace Million.Properties.Domain.Entities;

public class Property: BaseEntityModel
{
    public int IdProperty { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal Price { get; set; }
    public string CodeInternal { get; set; }
    public int Year { get; set; }
    public int? IdOwner { get; set; }
    public Owner? Owner { get; set; }
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();
}
