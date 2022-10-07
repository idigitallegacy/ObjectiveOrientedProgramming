using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private List<ShopProduct> _products = new ();

    public Shop(string name, string address, Guid id)
    {
        ValidateShopProperties(name, address);
        Id = id;
        Address = address;
        Name = name;
    }

    public string Name { get; }
    public string Address { get; }

    public Guid Id { get; }

    public void AddProducts(Supply supply)
    {
        _products.AddRange(supply.Products.Select(product => RegisterProduct(product)));
    }

    public void ChangePrice(Product shopProduct, decimal newPrice)
    {
        ShopProduct? needleProduct = FindShopProduct(shopProduct);
        if (needleProduct is null)
            throw new ShopException($"Unable to change price for {shopProduct.Name}: it's not presented at shop.");
        needleProduct.Properties.Price = new Price(newPrice);
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
        decimal cost = buyList.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum();
        if (person.Balance < cost)
            throw new ShopException($"Unable to buy products: not enough money.");
        GetMoney(person, cost);
        buyList.ForEach(item => ExecuteSell(item));
    }

    public Product? FindProduct(Product needleProduct)
    {
        return _products.Find(product => product.Name == needleProduct.Name);
    }

    public Product GetProduct(Product needleShopProduct)
    {
        return FindProduct(needleShopProduct) ??
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

    private void ValidateShopProperties(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ShopException("Unable to register shop with empty name");

        if (string.IsNullOrWhiteSpace(address))
            throw new ShopException("Unable to register shop with empty address");
    }

    private void GetMoney(Person person, decimal amount)
    {
        person.TakeMoney(amount);
    }

    private void ExecuteSell(ItemToBuy item)
    {
        GetShopProduct(item.Product).Properties.Amount.Value -= item.PreferredAmount;
    }

    private ShopProduct RegisterProduct(Product product)
    {
        return new ShopProduct(product.Name, product.Price.Value, product.Amount.Value);
    }

    private ShopProduct? FindShopProduct(Product needleProduct)
    {
        return _products.Find(product => product.Name == needleProduct.Name);
    }

    private ShopProduct GetShopProduct(Product needleShopProduct)
    {
        return FindShopProduct(needleShopProduct) ??
               throw new ShopException("Unable to get product: it's not at store.");
    }
}