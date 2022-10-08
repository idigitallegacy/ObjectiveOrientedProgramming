using Shops.Entities;

namespace Shops.Models;

public class Supply
{
    private List<SupplyRequest> _requests = new ();

    public Supply() { }

    private Supply(List<SupplyRequest> products)
    {
        _requests = new List<SupplyRequest>(products);
    }

    public IReadOnlyCollection<SupplyRequest> Requests => _requests;

    public void OrderProduct(SupplyRequest request)
    {
        _requests.Add(new SupplyRequest(request.Properties.Name, request.Properties.Price.Value, request.Properties.Amount.Value));
    }

    public object ShallowCopy()
    {
        return MemberwiseClone();
    }

    public Supply DeepCopy()
    {
        return new Supply(_requests);
    }
}