using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class ShopService
{
    private List<Shop> _shops = new ();

    public Shop RegisterShop(string name, string address)
    {
        Shop shop = new Shop(name, address, Guid.NewGuid());
        _shops.Add(shop);
        return shop;
    }

    public Product RegisterProduct(string name, float price, int amount)
    {
        return new Product(name, new Price(price), amount);
    }

    public void AddProducts(Shop shop, List<Product> products)
    {
        if (ValidateShop(shop))
            shop.AddProducts(products);
    }

    public void ChangePrice(Shop shop, Product product, float newPrice)
    {
        if (ValidateShop(shop))
            shop.ChangePrice(product, newPrice);
    }

    public Shop? FindCheapestShopToBuy(Product product)
    {
        List<Shop> availableShops = _shops
            .FindAll(shop => shop.FindProduct(product) is not null);

        if (availableShops.Count == 0)
            return null;

        var orderedShops = availableShops
            .Select((shop) => new { shop, product = shop.GetProduct(product) })
            .OrderBy(item => item.product.Price.Value);
        return orderedShops.First().shop;
    }

    public Shop? FindCheapestShopToBuy(List<ItemToBuy> buyList)
    {
        List<Shop> availableShops = _shops
            .FindAll(shop => buyList.TrueForAll(itemToBuy =>
            {
                Product? product = shop.FindProduct(itemToBuy.Product);
                return product is not null &&
                       product.Amount.Value >= itemToBuy.PreferredAmount;
            }));

        if (availableShops.Count == 0)
            return null;

        var shopsAndCost = availableShops
            .Select((shop) => new
            {
                shop,
                cost = shop.CalculatePrice(buyList),
            });

        var orderedShops = shopsAndCost.OrderBy(shopAndCost => shopAndCost.cost.Value);
        return orderedShops.First().shop;
    }

    private bool ValidateShop(Shop shop)
    {
        if (_shops.Find(shopItem => shopItem == shop) is null)
            throw new ShopServiceException("Unable to add products: shop not found.");
        return true;
    }
}