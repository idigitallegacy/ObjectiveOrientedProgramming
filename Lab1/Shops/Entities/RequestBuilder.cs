using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class RequestBuilder
{
    private string _name = string.Empty;
    private Price? _price = null;
    private ProductAmount? _amount = null;
    private SupplyRequest? _request = null;

    public RequestBuilder()
    {
        Reset();
    }

    public RequestBuilder SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new RequestException("Name should be filled.");
        _name = name;
        return this;
    }

    public RequestBuilder SetPrice(decimal price)
    {
        _price = new Price(price);
        return this;
    }

    public RequestBuilder SetAmount(int amount)
    {
        _amount = new ProductAmount(amount);
        return this;
    }

    public SupplyRequest Build()
    {
        if (_name == string.Empty || _price is null || _amount is null)
            throw new RequestException("Unable to build inconsistent object");
        _request = new SupplyRequest(_name, _price.Value, _amount.Value);

        SupplyRequest result = _request.DeepCopy();
        Reset();
        return result;
    }

    public void Reset()
    {
        _name = string.Empty;
        _price = null;
        _amount = null;
        _request = null;
    }
}