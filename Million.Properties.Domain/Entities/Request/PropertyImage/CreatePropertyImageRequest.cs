
namespace Million.Properties.Domain.Entities.Request.PropertyImage;

public class CreatePropertyImageRequest
{
    public int IdProperty { get; set; }
    public string File { get; set; } = string.Empty; 
}
