
namespace Million.Properties.Domain.Common;

public class BaseEntityModel
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
}
