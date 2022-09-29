using Shops.Exceptions;

namespace Shops.Models;

public class Price
{
    private float _value;
    public Price(float price)
    {
        if (price < 0)
            throw new PriceException("Price mustn't be negative.");
        _value = price;
    }

    public float Value
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