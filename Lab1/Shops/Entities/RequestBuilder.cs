using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class RequestBuilder
{
    private SupplyRequest? _request = null;

    public SupplyRequest Request
    {
        get => _request ?? throw new RequestException("Unable to get object that hasn't been built yet.");
        private set => _request = value;
    }

    public void Build(string name, decimal price, int amount)
    {
        Request = new SupplyRequest(name, price, amount);
    }
}