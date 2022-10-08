namespace Shops.Models;

public class SupplyRequest
{
    public SupplyRequest(string name, decimal price, int amount)
    {
        Properties = new ProductProperties(name, price, amount);
    }

    public ProductProperties Properties { get; }

    public object ShallowCopy()
    {
        return MemberwiseClone();
    }

    public SupplyRequest DeepCopy()
    {
        return new SupplyRequest(Properties.Name, Properties.Price.Value, Properties.Amount.Value);
    }
}