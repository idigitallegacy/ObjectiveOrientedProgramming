using Shops.Exceptions;

namespace Shops.Models;

public class ProductAmount
{
    private int _value;

    public ProductAmount(int amount)
    {
        Value = amount;
    }

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value < 0)
                throw new ProductAmountException("ProductAmount must be grater than 0.");
            _value = value;
        }
    }
}