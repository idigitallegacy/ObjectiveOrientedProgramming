using Shops.Exceptions;

namespace Shops.Models;

public class ShopProduct : Product
{
    public ShopProduct(string name, decimal price, int amount)
    : base(new ProductProperties(name, price, amount))
    {
        Properties = base.Properties;
    }

    public new ProductProperties Properties { get; }
}