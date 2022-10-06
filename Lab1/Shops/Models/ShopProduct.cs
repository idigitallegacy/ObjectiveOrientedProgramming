using Shops.Exceptions;

namespace Shops.Models;

public class ShopProduct : Product
{
    public ShopProduct(string name, Price price, int amount)
    : base(new ProductProperties(name, price, new ProductAmount(amount)))
    {
        Properties = base.Properties;
    }

    public new ProductProperties Properties { get; }
}