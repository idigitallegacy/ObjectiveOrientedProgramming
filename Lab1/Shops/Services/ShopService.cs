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

    public ShopProduct RegisterProduct(string name, int price, int amount)
    {
        return new ShopProduct(name, new Price(price), amount);
    }

    public void AddProducts(Shop shop, List<Product> products)
    {
        if (ValidateShop(shop))
            shop.AddProducts(products);
    }

    public void ChangePrice(Shop shop, Product shopProduct, int newPrice)
    {
        if (ValidateShop(shop))
            shop.ChangePrice(shopProduct, newPrice);
    }

    public Shop? FindCheapestShopToBuy(List<ItemToBuy> buyList)
    {
        List<Shop> availableShops = FindAvailableShops(buyList);

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

    public Shop? FindCheapestShopToBuy(ShopProduct shopProduct, int preferredAmount)
    {
        List<ItemToBuy> singleItemList = new List<ItemToBuy> { new (shopProduct, preferredAmount) };
        return FindCheapestShopToBuy(singleItemList);
    }

    private bool ValidateShop(Shop shop)
    {
        if (_shops.Find(shopItem => shopItem == shop) is null)
            throw new ShopServiceException("Unable to add products: shop not found.");
        return true;
    }

    private List<Shop> FindAvailableShops(List<ItemToBuy> buyList)
    {
        return _shops.FindAll(shop => buyList.TrueForAll(item =>
        {
            Product? product = shop.FindProduct(item.Product);
            return product is not null && item.PreferredAmount <= product.Amount.Value;
        }));
    }
}