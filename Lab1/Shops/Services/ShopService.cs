using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class ShopService
{
    private List<Shop> _shops = new ();

    private List<Product> _allProducts = new ();

    public Shop RegisterShop(string name, string address)
    {
        Shop shop = new Shop(name, address, Guid.NewGuid());
        _shops.Add(shop);
        return shop;
    }

    public SupplyRequest RegisterProduct(string name, int price, int amount)
    {
        if (_allProducts.Find(product => product.Name == name) is not null)
            throw new ShopServiceException("Unable to add product: it's already exists.");

        SupplyRequest request = new SupplyRequest(name, price, amount);

        _allProducts.Add(new Product(request.Properties));
        return request;
    }

    public void AddProducts(Shop shop, Supply supply)
    {
        if (ValidateShop(shop))
            shop.AddProducts(supply);
    }

    public void ChangePrice(Shop shop, Product product, int newPrice)
    {
        if (ValidateShop(shop))
            shop.ChangePrice(product, newPrice);
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

    public Shop? FindCheapestShopToBuy(Product product, int preferredAmount)
    {
        List<ItemToBuy> singleItemList = new List<ItemToBuy> { new (product, preferredAmount) };
        return FindCheapestShopToBuy(singleItemList);
    }

    public Product? FindProduct(ProductProperties properties)
    {
        return _allProducts.Find(product => product.Name == properties.Name);
    }

    public Product GetProduct(ProductProperties properties)
    {
        return FindProduct(properties) ?? throw new ShopServiceException($"Unable to get product {properties.Name}");
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