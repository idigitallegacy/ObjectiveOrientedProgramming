namespace Shops.Models;

public class SupplyRequest
{
    public SupplyRequest(string name, decimal price, int amount)
    {
        Properties = new ProductProperties(name, price, amount);
    }

    public ProductProperties Properties { get; }
}