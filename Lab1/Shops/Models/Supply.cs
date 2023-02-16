using Shops.Entities;

namespace Shops.Models;

public class Supply
{
    private List<SupplyRequest> _requests = new ();

    public Supply(List<SupplyRequest> products)
    {
        _requests = new List<SupplyRequest>(products);
    }

    public IReadOnlyCollection<SupplyRequest> Requests => _requests;
}