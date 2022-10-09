using Shops.Exceptions;
using Shops.Models;
using Shops.Services;

namespace Shops.Entities;

public class SupplyBuilder
{
    private List<SupplyRequest> _requests = new ();

    public SupplyBuilder()
    {
        Reset();
    }

    public SupplyBuilder AddRequest(SupplyRequest request)
    {
        _requests.Add(request);
        return this;
    }

    public Supply Build()
    {
        Supply result = new Supply(_requests);
        Reset();
        return result;
    }

    public void Reset()
    {
        _requests = new ();
    }
}