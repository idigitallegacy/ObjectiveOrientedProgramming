namespace Shops.Models;

public class Product
{
    public Product(string name, Price price, int amount)
    {
        Name = name;
        Price = price;
        Amount = new ProductAmount(amount);
    }

    public string Name { get; }

    public Price Price { get; set; }

    public ProductAmount Amount { get; set; }
}