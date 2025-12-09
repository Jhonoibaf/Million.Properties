using Million.Properties.Domain.Common;

namespace Million.Properties.Domain.Entities;

public class Owner : BaseEntityModel
{
    public int IdOwner { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Photo { get; set; }
    public DateTime Birthday { get; set; }
    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
