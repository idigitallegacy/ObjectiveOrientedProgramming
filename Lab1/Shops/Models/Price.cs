using Shops.Exceptions;

namespace Shops.Models;

public class Price
{
    private int _value;
    public Price(int price)
    {
        Value = price;
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
                throw new PriceException("Price mustn't be negative.");
            _value = value;
        }
    }
}