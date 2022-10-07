using Shops.Exceptions;

namespace Shops.Models;

public class ProductProperties
{
    public ProductProperties(string name, decimal price, int amount)
    {
        if (string.IsNullOrEmpty(name))
            throw new ProductPropertyException("Invalid product name.");
        Name = name;
        Price = new Price(price);
        Amount = new ProductAmount(amount);
    }

    public string Name { get; }
    public Price Price { get; set; }

    public ProductAmount Amount { get; set; }
}