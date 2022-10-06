using Shops.Exceptions;

namespace Shops.Models;

public class ItemToBuy
{
    public ItemToBuy(Product serviceProduct, int preferredAmount)
    {
        if (preferredAmount < 0)
            throw new ProductAmountException("Unable to generate item to buy: preferred amount must be above 0.");
        Product = serviceProduct;
        PreferredAmount = preferredAmount;
    }

    public Product Product { get; }

    public int PreferredAmount { get; }
}