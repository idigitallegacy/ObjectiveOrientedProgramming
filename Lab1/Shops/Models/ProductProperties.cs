using Shops.Exceptions;

namespace Shops.Models;

public class ProductProperties
{
    public ProductProperties(string name, Price price, ProductAmount amount)
    {
        if (string.IsNullOrEmpty(name))
            throw new ProductPropertyException("Invalid product name.");
        Name = name;
        Price = price;
        Amount = amount;
    }

    public string Name { get; }
    public Price Price { get; set; }

    public ProductAmount Amount { get; set; }
}