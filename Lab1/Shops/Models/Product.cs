using Shops.Exceptions;

namespace Shops.Models;

public class Product
{
    public Product(ProductProperties properties)
    {
        Properties = properties;
    }

    public string Name { get => Properties.Name; }

    public Price Price { get => Properties.Price; }

    public ProductAmount Amount { get => Properties.Amount; }

    protected ProductProperties Properties { get; }
}