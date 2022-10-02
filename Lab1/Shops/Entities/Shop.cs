using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private List<Product> _products = new ();

    public Shop(string name, string address, Guid id)
    {
        Id = id;
        Address = address;
        Name = name;
    }

    public string Name { get; }
    public string Address { get; }

    public Guid Id { get; }

    public void AddProducts(List<Product> products)
    {
        // new Product(...) is to keep different prices and amounts around all shops
        _products.AddRange(products.Select(product => new Product(product.Name, product.Price, product.Amount.Value)));
    }

    public void ChangePrice(Product product, int newPrice)
    {
        Product? needleProduct = FindProduct(product);
        if (needleProduct is null)
            throw new ShopException($"Unable to change price for {product.Name}: it's not presented at shop.");
        needleProduct.Price = new Price(newPrice);
    }

    public void BuyProduct(Person person, Product product, int preferredAmount)
    {
        ItemToBuy item = new ItemToBuy(product, preferredAmount);
        ValidateSell(person, item);
        GetMoney(person, product.Price.Value * preferredAmount);
        ExecuteSell(item);
    }

    public void BuyProducts(Person person, List<ItemToBuy> buyList)
    {
        buyList.ForEach(item => ValidateSell(person, item));
        int cost = buyList.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum();
        if (person.Balance < cost)
            throw new ShopException($"Unable to buy products: not enough money.");
        GetMoney(person, cost);
        buyList.ForEach(item => ExecuteSell(item));
    }

    public Product? FindProduct(Product needleProduct)
    {
        return _products.Find(product => product.Name == needleProduct.Name);
    }

    public Product GetProduct(Product needleProduct)
    {
        return FindProduct(needleProduct) ??
               throw new ShopException("Unable to get product: it's not at store.");
    }

    public Price CalculatePrice(List<ItemToBuy> buyList)
    {
        if (!buyList.TrueForAll(item =>
            {
                Product? product = FindProduct(item.Product);
                return product is not null &&
                       product.Amount.Value >= item.PreferredAmount;
            }))
            throw new ShopException("Unable to calculate price: not enough requested products or requested products are not at shop.");

        return new Price(buyList.Select(item => GetProduct(item.Product).Price.Value * item.PreferredAmount).Sum());
    }

    private void ValidateSell(Person person, ItemToBuy item)
    {
        Product? product = FindProduct(item.Product);
        if (product is null)
            throw new ShopException($"Unable to buy product {item.Product.Name}: it is not found at store.");

        if (product.Amount.Value < item.PreferredAmount)
            throw new ShopException($"Unable to buy product {item.Product.Name}: it is not enough at store.");

        if (person.Balance < item.PreferredAmount * product.Price.Value)
            throw new ShopException($"Unable to buy product {item.Product.Name}: not enough money.");
    }

    private void GetMoney(Person person, int amount)
    {
        person.TakeMoney(amount);
    }

    private void ExecuteSell(ItemToBuy item)
    {
        GetProduct(item.Product).Amount.Value -= item.PreferredAmount;
    }
}