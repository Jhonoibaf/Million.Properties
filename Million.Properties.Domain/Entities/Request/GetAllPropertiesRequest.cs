namespace Million.Properties.Domain.Entities.Request;

public class GetAllPropertiesRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
