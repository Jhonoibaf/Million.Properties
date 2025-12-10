namespace Million.Properties.Domain.Entities.Request.Properties;

public class UpdatePropertyPriceByIdCommandRequest
{
    public int IdProperty { get; set; }
    public decimal NewPrice { get; set; }

    public UpdatePropertyPriceByIdCommandRequest(int idProperty, decimal newPrice)
    {
        IdProperty = idProperty;
        NewPrice = newPrice;
    }
}
