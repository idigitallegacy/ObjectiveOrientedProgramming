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

    public void ChangePrice(Product product, float newPrice)
    {
        Product? needleProduct = FindProduct(product);
        if (needleProduct is null)
            throw new ShopException($"Unable to change price for {product.Name}: it's not presented at shop.");
        needleProduct.Price = new Price(newPrice);
    }

    public void BuyProduct(Person person, Product product, int preferredAmount)
    {
        List<ItemToBuy> buyList = new ();
        buyList.Add(new ItemToBuy(GetProduct(product), preferredAmount));

        if (ValidateSell(person, buyList))
        {
            GetMoney(person, product.Price.Value * preferredAmount);
            ExecuteSell(buyList);
        }
    }

    public void BuyProducts(Person person, List<ItemToBuy> buyList)
    {
        if (ValidateSell(person, buyList))
        {
            float cost = buyList.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum();
            GetMoney(person, cost);
            ExecuteSell(buyList);
        }
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

    private bool ValidateSell(Person toPerson, List<ItemToBuy> buyList)
    {
        if (buyList.Any(item => FindProduct(item.Product) is null))
            throw new ShopException("Unable to buy multiple products: one or more product is not found at store.");

        if (buyList.Any(item => item.Product.Amount.Value < item.PreferredAmount))
            throw new ShopException("Unable to buy multiple products: one or more product is not enough at store.");

        float requestedCost = buyList.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum();
        if (toPerson.Balance < requestedCost)
            throw new ShopException($"Unable to buy products: not enough money.");

        return true;
    }

    private void GetMoney(Person person, float amount)
    {
        person.GiveMoney(amount);
    }

    private void ExecuteSell(List<ItemToBuy> buyList)
    {
        buyList.ForEach(item => { GetProduct(item.Product).Amount.Value -= item.PreferredAmount; });
    }
}