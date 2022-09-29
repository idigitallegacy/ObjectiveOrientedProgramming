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

    public static bool operator !=(Product product1, Product product2)
    {
        return product1.Name != product2.Name;
    }

    public static bool operator ==(Product product1, Product product2)
    {
        return product1.Name == product2.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Product) return (Product)obj == this;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Price, Amount);
    }
}