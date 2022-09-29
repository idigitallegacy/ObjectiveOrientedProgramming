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
        const float productPrice = 10.5f;
        const int productAmount = 100;

        const float moneyBefore = 500f;
        const int preferredAmount = 20;

        const string shopName = "Shop #1";
        const string shopAddress = "abc,49";

        var shop = shopService.RegisterShop(shopName, shopAddress);
        var product = shopService.RegisterProduct(productName, productPrice, productAmount);
        var productList = new List<Product>();
        productList.Add(product);
        person.TakeMoney(moneyBefore);

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
        const float productPrice = 10.5f;
        const int productAmount = 100;

        const float moneyBefore = 500f;
        const float newPrice = 20;

        const string shop1Name = "Shop #1";
        const string shop2Name = "Shop #1";
        const string shopAddress = "abc,49";

        var shop1 = shopService.RegisterShop(shop1Name, shopAddress);
        var shop2 = shopService.RegisterShop(shop2Name, shopAddress);
        var product = shopService.RegisterProduct(productName, productPrice, productAmount);
        var productList = new List<Product>();
        productList.Add(product);
        person.TakeMoney(moneyBefore);

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
        var person = new Person("Michael");
        var shopService = new ShopService();

        const string product1Name = "Product #1";
        const string product2Name = "Product #2";
        const string product3Name = "Product #3";
        const string product4Name = "Product #4";
        const string product5Name = "Product #5";
        const string product6Name = "Product #6";
        const float price1 = 10f;
        const float price2 = 20f;
        const float price3 = 30f;
        const float price4 = 40f;
        const float price5 = 50f;
        const float price6 = 60f;
        const int productAmount = 100;

        const float moneyBefore = 500f;
        const int preferredAmount = 10;

        const string shop1Name = "Shop #1";
        const string shop2Name = "Shop #2";
        const string shop3Name = "Shop #3";
        const string shopAddress = "abc,49";

        var shop1 = shopService.RegisterShop(shop1Name, shopAddress);
        var shop2 = shopService.RegisterShop(shop2Name, shopAddress);
        var shop3 = shopService.RegisterShop(shop3Name, shopAddress);

        var product1 = shopService.RegisterProduct(product1Name, price1, productAmount);
        var product2 = shopService.RegisterProduct(product2Name, price2, productAmount);
        var product3 = shopService.RegisterProduct(product3Name, price3, productAmount);
        var product4 = shopService.RegisterProduct(product4Name, price4, productAmount);
        var product5 = shopService.RegisterProduct(product5Name, price5, productAmount);
        var product6 = shopService.RegisterProduct(product6Name, price6, productAmount);

        var productListFull = new List<Product>();
        productListFull.Add(product1);
        productListFull.Add(product2);
        productListFull.Add(product3);
        productListFull.Add(product4);
        productListFull.Add(product5);
        productListFull.Add(product6);
        person.TakeMoney(moneyBefore);

        var listToBuy = new List<ItemToBuy>();
        listToBuy.Add(new ItemToBuy(product1, preferredAmount));
        listToBuy.Add(new ItemToBuy(product2, preferredAmount));
        listToBuy.Add(new ItemToBuy(product3, preferredAmount));

        shopService.AddProducts(shop1, productListFull);
        shopService.AddProducts(shop2, productListFull);
        shopService.AddProducts(shop3, productListFull);

        shopService.ChangePrice(shop1, product1, price2);
        shopService.ChangePrice(shop1, product2, price2);
        shopService.ChangePrice(shop1, product3, price2);

        shopService.ChangePrice(shop2, product1, price3);
        shopService.ChangePrice(shop2, product2, price3);
        shopService.ChangePrice(shop2, product3, price3);

        var foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shop1.Id, foundShop?.Id);

        shopService.ChangePrice(shop3, product1, price1);
        shopService.ChangePrice(shop3, product2, price1);
        shopService.ChangePrice(shop3, product3, price1);

        foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shop3.Id, foundShop?.Id);
    }

    [Fact]
    public void SearchCheapestShop_SomeShopsHaveNotGotEnoughProducts()
    {
        var person = new Person("Michael");
        var shopService = new ShopService();

        const string product1Name = "Product #1";
        const string product2Name = "Product #2";
        const string product3Name = "Product #3";
        const string product4Name = "Product #4";
        const string product5Name = "Product #5";
        const string product6Name = "Product #6";
        const float price1 = 10f;
        const float price2 = 20f;
        const float price3 = 30f;
        const float price4 = 40f;
        const float price5 = 50f;
        const float price6 = 60f;
        const int productAmount = 30;

        const float moneyBefore = 50000f;
        const int preferredAmount1 = 10;
        const int preferredAmount2 = 20;
        const int preferredAmount3 = 30;

        const string shop1Name = "Shop #1";
        const string shop2Name = "Shop #2";
        const string shop3Name = "Shop #3";
        const string shopAddress = "abc,49";

        var shop1 = shopService.RegisterShop(shop1Name, shopAddress);
        var shop2 = shopService.RegisterShop(shop2Name, shopAddress);
        var shop3 = shopService.RegisterShop(shop3Name, shopAddress);

        var product1 = shopService.RegisterProduct(product1Name, price1, productAmount);
        var product2 = shopService.RegisterProduct(product2Name, price2, productAmount);
        var product3 = shopService.RegisterProduct(product3Name, price3, productAmount);
        var product4 = shopService.RegisterProduct(product4Name, price4, productAmount);
        var product5 = shopService.RegisterProduct(product5Name, price5, productAmount);
        var product6 = shopService.RegisterProduct(product6Name, price6, productAmount);

        var productListFull = new List<Product>();
        productListFull.Add(product1);
        productListFull.Add(product2);
        productListFull.Add(product3);
        productListFull.Add(product4);
        productListFull.Add(product5);
        productListFull.Add(product6);
        person.TakeMoney(moneyBefore);

        var listToBuy = new List<ItemToBuy>();
        listToBuy.Add(new ItemToBuy(product1, preferredAmount3));
        listToBuy.Add(new ItemToBuy(product2, preferredAmount3));
        listToBuy.Add(new ItemToBuy(product3, preferredAmount3));

        shopService.AddProducts(shop1, productListFull);
        shopService.AddProducts(shop2, productListFull);
        shopService.AddProducts(shop3, productListFull);

        shop1.BuyProduct(person, product1, preferredAmount1);
        shop2.BuyProduct(person, product2, preferredAmount2);

        shopService.ChangePrice(shop1, product1, price1);
        shopService.ChangePrice(shop1, product2, price1);
        shopService.ChangePrice(shop1, product3, price1);

        shopService.ChangePrice(shop2, product1, price1);
        shopService.ChangePrice(shop2, product2, price1);
        shopService.ChangePrice(shop2, product3, price1);

        var foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Equal(shop3.Id, foundShop?.Id);

        shop3.BuyProduct(person, product1, preferredAmount1);
        foundShop = shopService.FindCheapestShopToBuy(listToBuy);

        Assert.Null(foundShop);
    }

    [Fact]
    public void BuyMultipleProducts_NotEnoughMoneyOrProducts()
    {
        var person = new Person("Michael");
        var shopService = new ShopService();

        const string product1Name = "Product #1";
        const string product2Name = "Product #2";
        const string product3Name = "Product #3";
        const string product4Name = "Product #4";
        const string product5Name = "Product #5";
        const string product6Name = "Product #6";
        const float price1 = 10f;
        const float price2 = 20f;
        const float price3 = 30f;
        const float price4 = 40f;
        const float price5 = 50f;
        const float price6 = 60f;
        const int productAmount = 30;

        const float moneyBefore = 1000f;
        const int preferredAmount1 = 10;
        const int preferredAmount2 = 30;

        const string shop1Name = "Shop #1";
        const string shop2Name = "Shop #2";
        const string shop3Name = "Shop #3";
        const string shopAddress = "abc,49";

        var shop1 = shopService.RegisterShop(shop1Name, shopAddress);
        var shop2 = shopService.RegisterShop(shop2Name, shopAddress);
        var shop3 = shopService.RegisterShop(shop3Name, shopAddress);

        var product1 = shopService.RegisterProduct(product1Name, price1, productAmount);
        var product2 = shopService.RegisterProduct(product2Name, price2, productAmount);
        var product3 = shopService.RegisterProduct(product3Name, price3, productAmount);
        var product4 = shopService.RegisterProduct(product4Name, price4, productAmount);
        var product5 = shopService.RegisterProduct(product5Name, price5, productAmount);
        var product6 = shopService.RegisterProduct(product6Name, price6, productAmount);

        var productListFull = new List<Product>();
        productListFull.Add(product1);
        productListFull.Add(product2);
        productListFull.Add(product3);
        productListFull.Add(product4);
        productListFull.Add(product5);
        productListFull.Add(product6);
        person.TakeMoney(moneyBefore);

        var incorrectListToBuy = new List<ItemToBuy>();
        incorrectListToBuy.Add(new ItemToBuy(product1, preferredAmount2));
        incorrectListToBuy.Add(new ItemToBuy(product2, preferredAmount2));
        incorrectListToBuy.Add(new ItemToBuy(product3, preferredAmount2));

        var correctListToBuy = new List<ItemToBuy>();
        correctListToBuy.Add(new ItemToBuy(product1, preferredAmount1));
        correctListToBuy.Add(new ItemToBuy(product2, preferredAmount1));
        correctListToBuy.Add(new ItemToBuy(product3, preferredAmount1));

        shopService.AddProducts(shop1, productListFull);
        shopService.AddProducts(shop2, productListFull);
        shopService.AddProducts(shop3, productListFull);

        shopService.ChangePrice(shop1, product1, price1);
        shopService.ChangePrice(shop1, product2, price1);
        shopService.ChangePrice(shop1, product3, price1);

        shopService.ChangePrice(shop2, product1, price1);
        shopService.ChangePrice(shop2, product2, price1);
        shopService.ChangePrice(shop2, product3, price1);

        var foundShop = shopService.FindCheapestShopToBuy(incorrectListToBuy);
        var ex = Assert.Throws<ShopException>(() => foundShop?.BuyProducts(person, incorrectListToBuy));
        Assert.Equal("Unable to buy products: not enough money.", ex.Message);

        foundShop = shopService.FindCheapestShopToBuy(correctListToBuy);
        foundShop?.BuyProducts(person, correctListToBuy);
        Assert.Equal(moneyBefore - correctListToBuy.Select(item => item.Product.Price.Value * item.PreferredAmount).Sum(), person.Balance);
        Assert.Equal(productAmount - preferredAmount1, foundShop !.GetProduct(product1).Amount.Value);
        Assert.Equal(productAmount - preferredAmount1, foundShop !.GetProduct(product2).Amount.Value);
        Assert.Equal(productAmount - preferredAmount1, foundShop !.GetProduct(product3).Amount.Value);

        person.TakeMoney(500f);
        ex = Assert.Throws<ShopException>(() => shop1.BuyProduct(person, product1, preferredAmount2));
        Assert.Equal("Unable to buy multiple products: one or more product is not enough at store.", ex.Message);
    }
}