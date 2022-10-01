using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopServiceTest
{
    [Fact]
    public void AddProductsToShop_BuyProduct_BalanceAndProductAmountAreChanged()
    {
        var person = new Person("Michael");
        var shopService = new ShopService();

        const string productName = "Product #1";
        const int productPrice = 10;
        const int productAmount = 100;

        const int moneyBefore = 500;
        const int preferredAmount = 20;

        const string shopName = "Shop #1";
        const string shopAddress = "abc,49";

        var shop = shopService.RegisterShop(shopName, shopAddress);
        var product = shopService.RegisterProduct(productName, productPrice, productAmount);
        var productList = new List<Product>();
        productList.Add(product);
        person.GiveMoney(moneyBefore);

        shopService.AddProducts(shop, productList);

        shop.BuyProduct(person, product, preferredAmount);

        Assert.Equal(moneyBefore - (preferredAmount * productPrice), person.Balance);
        Assert.Equal(productAmount - preferredAmount, shop.GetProduct(product).Amount.Value);
    }

    [Fact]
    public void ChangeProductPrice_PriceChanged()
    {
        var person = new Person("Michael");
        var shopService = new ShopService();

        const string productName = "Product #1";
        const int productPrice = 10;
        const int productAmount = 100;

        const int moneyBefore = 50;
        const int newPrice = 20;

        const string shop1Name = "Shop #1";
        const string shop2Name = "Shop #1";
        const string shopAddress = "abc,49";

        var shop1 = shopService.RegisterShop(shop1Name, shopAddress);
        var shop2 = shopService.RegisterShop(shop2Name, shopAddress);
        var product = shopService.RegisterProduct(productName, productPrice, productAmount);
        var productList = new List<Product>();
        productList.Add(product);
        person.GiveMoney(moneyBefore);

        shopService.AddProducts(shop1, productList);
        shopService.AddProducts(shop2, productList);

        shopService.ChangePrice(shop1, product, newPrice);

        Assert.Equal(newPrice, shop1.GetProduct(product).Price.Value);
        Assert.NotEqual(newPrice, shop2.GetProduct(product).Price.Value);
        Assert.NotEqual(newPrice, product.Price.Value);
    }

    [Fact]
    public void SearchCheapestShop_ShopFound()
    {
        const int productNamesAmount = 6;
        const int productsToBuyAmount = 3;
        const int productAmount = 100;

        var shopsAmount = 3;
        const string shopAddress = "abc,49";

        const int moneyBefore = 50;
        const int preferredAmount = 10;

        var productNames = new List<string>();
        var prices = new List<int>();
        var products = new List<Product>();

        var shopNames = new List<string>();
        var shops = new List<Shop>();

        var person = new Person("Michael");
        var shopService = new ShopService();

        var productListFull = new List<Product>();
        var listToBuy = new List<ItemToBuy>();

        for (int productName = 0; productName < productNamesAmount; productName++)
            productNames.Add($"Product #{productName}");

        for (int price = 1; price < productNamesAmount + 1; price++)
            prices.Add(price);

        for (int shopName = 0; shopName < shopsAmount; shopName++)
            shopNames.Add($"Shop #{shopName}");

        foreach (var shopName in shopNames)
            shops.Add(shopService.RegisterShop(shopName, shopAddress));

        for (int i = 0; i < productNamesAmount; i++)
            products.Add(shopService.RegisterProduct(productNames[i], prices[i], productAmount));

        for (var i = 0; i < productNamesAmount; i++)
            productListFull.Add(products[i]);

        for (var i = 0; i < productsToBuyAmount; i++)
            listToBuy.Add(new ItemToBuy(products[i], preferredAmount));

        for (var i = 0; i < shopsAmount; i++)
            shopService.AddProducts(shops[i], productListFull);

        for (var i = 0; i < productsToBuyAmount; i++)
            shopService.ChangePrice(shops[0], products[i], prices[1]);

        for (var i = 0; i < productsToBuyAmount; i++)
            shopService.ChangePrice(shops[1], products[i], prices[2]);

        person.GiveMoney(moneyBefore);
        var foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shops[0].Id, foundShop?.Id);

        for (var i = 0; i < productsToBuyAmount; i++)
            shopService.ChangePrice(shops[2], products[i], prices[0]);

        foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shops[2].Id, foundShop?.Id);
    }

    [Fact]
    public void SearchCheapestShop_SomeShopsHaveNotGotEnoughProducts()
    {
        const int productNamesAmount = 6;
        const int productsToBuyAmount = 3;
        const int productAmount = 50;

        var shopsAmount = 3;
        const string shopAddress = "abc,49";

        const int moneyBefore = 5000;

        const int preferredAmount = 30;

        var productNames = new List<string>();
        var prices = new List<int>();
        var products = new List<Product>();

        var shopNames = new List<string>();
        var shops = new List<Shop>();

        var person = new Person("Michael");
        var shopService = new ShopService();

        var productListFull = new List<Product>();
        var listToBuy = new List<ItemToBuy>();

        for (int productName = 0; productName < productNamesAmount; productName++)
            productNames.Add($"Product #{productName}");

        for (int price = 1; price < productNamesAmount + 1; price++)
            prices.Add(price);

        for (int shopName = 0; shopName < shopsAmount; shopName++)
            shopNames.Add($"Shop #{shopName}");

        foreach (var shopName in shopNames)
            shops.Add(shopService.RegisterShop(shopName, shopAddress));

        for (int i = 0; i < productNamesAmount; i++)
            products.Add(shopService.RegisterProduct(productNames[i], prices[i], productAmount));

        for (var i = 0; i < productNamesAmount; i++)
            productListFull.Add(products[i]);

        for (var i = 0; i < productsToBuyAmount; i++)
            listToBuy.Add(new ItemToBuy(products[i], preferredAmount));

        for (var i = 0; i < shopsAmount; i++)
            shopService.AddProducts(shops[i], productListFull);

        person.GiveMoney(moneyBefore);

        shops[0].BuyProduct(person, products[0], preferredAmount);
        shops[1].BuyProduct(person, products[0], preferredAmount);

        for (var i = 0; i < productsToBuyAmount; i++)
            shopService.ChangePrice(shops[0], products[i], prices[3]);

        for (var i = 0; i < productsToBuyAmount; i++)
            shopService.ChangePrice(shops[2], products[i], prices[1]);

        var foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shops[2].Id, foundShop?.Id);

        shops[2].BuyProduct(person, products[0], preferredAmount);
        foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Null(foundShop);
    }

    [Fact]
    public void BuyMultipleProducts_NotEnoughMoneyOrProducts()
    {
        const int productNamesAmount = 6;
        const int productsToBuyAmount = 3;
        const int productAmount = 30;
        const int shopsAmount = 3;
        const int price = 100;

        const string shopAddress = "abc,49";

        const int moneyBefore = 4000;

        const int preferredAmount1 = 10;
        const int preferredAmount2 = 30;

        var productNames = new List<string>();
        var products = new List<Product>();

        var shopNames = new List<string>();
        var shops = new List<Shop>();

        var person = new Person("Michael");
        var shopService = new ShopService();

        var productListFull = new List<Product>();
        var incorrectListToBuy = new List<ItemToBuy>();
        var correctListToBuy = new List<ItemToBuy>();

        for (int productName = 0; productName < productNamesAmount; productName++)
            productNames.Add($"Product #{productName}");

        for (int shopName = 0; shopName < shopsAmount; shopName++)
            shopNames.Add($"Shop #{shopName}");

        foreach (var shopName in shopNames)
            shops.Add(shopService.RegisterShop(shopName, shopAddress));

        for (int i = 0; i < productNamesAmount; i++)
            products.Add(shopService.RegisterProduct(productNames[i], price, productAmount));

        for (var i = 0; i < productNamesAmount; i++)
            productListFull.Add(products[i]);

        for (var i = 0; i < productsToBuyAmount; i++)
        {
            correctListToBuy.Add(new ItemToBuy(products[i], preferredAmount1));
            incorrectListToBuy.Add(new ItemToBuy(products[i], preferredAmount2));
        }

        for (var i = 0; i < shopsAmount; i++)
            shopService.AddProducts(shops[i], productListFull);

        person.GiveMoney(moneyBefore);

        var foundShop = shopService.FindCheapestShopToBuy(incorrectListToBuy);
        var ex = Assert.Throws<ShopException>(() => foundShop?.BuyProducts(person, incorrectListToBuy));
        Assert.Equal("Unable to buy products: not enough money.", ex.Message);

        foundShop = shopService.FindCheapestShopToBuy(correctListToBuy);
        foundShop?.BuyProducts(person, correctListToBuy);
        Assert.Equal(moneyBefore - correctListToBuy.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum(), person.Balance);
        Assert.Equal(productAmount - preferredAmount1, foundShop?.GetProduct(products[0]).Amount.Value);
        Assert.Equal(productAmount - preferredAmount1, foundShop?.GetProduct(products[1]).Amount.Value);
        Assert.Equal(productAmount - preferredAmount1, foundShop?.GetProduct(products[2]).Amount.Value);

        person.GiveMoney(5000);
        ex = Assert.Throws<ShopException>(() => shops[0].BuyProduct(person, products[0], preferredAmount2));
        Assert.Equal("Unable to buy multiple products: one or more product is not enough at store.", ex.Message);
    }
}