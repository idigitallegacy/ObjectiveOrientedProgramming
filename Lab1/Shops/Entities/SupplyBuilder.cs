using Shops.Exceptions;
using Shops.Models;
using Shops.Services;

namespace Shops.Entities;

public class SupplyBuilder
{
    private List<SupplyRequest> _requests = new ();
    private Supply? _supply = null;

    public SupplyBuilder()
    {
        Reset();
    }

    public SupplyBuilder AddRequest(SupplyRequest request)
    {
        _requests.Add(request);
        return this;
    }

    public SupplyBuilder AddRequests(List<SupplyRequest> requests)
    {
        _requests.AddRange(requests);
        return this;
    }

    public Supply Build()
    {
        if (_supply is null)
            _supply = new Supply();
        _requests.ForEach(request => _supply.OrderProduct(request));

        Supply result = _supply.DeepCopy();
        Reset();
        return result;
    }

    public void Reset()
    {
        _requests = new ();
        _supply = null;
    }
}