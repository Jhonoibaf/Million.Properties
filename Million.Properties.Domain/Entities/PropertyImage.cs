using Million.Properties.Domain.Common;

namespace Million.Properties.Domain.Entities;

public class PropertyImage: BaseEntityModel
{

    public int IdPropertyImage { get; set; }
    public string File { get; set; }
    public bool Enabled { get; set; }
    public int IdProperty { get; set; }   
    public Property Property { get; set; }
}
