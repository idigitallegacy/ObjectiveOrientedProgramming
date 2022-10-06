using Shops.Exceptions;

namespace Shops.Models;

public class Price
{
    private decimal _value;
    public Price(decimal price)
    {
        Value = price;
    }

    public decimal Value
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