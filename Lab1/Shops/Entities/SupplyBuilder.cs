using Shops.Exceptions;
using Shops.Models;
using Shops.Services;

namespace Shops.Entities;

public class SupplyBuilder
{
    public Supply Supply { get; } = new ();

    public void Build(SupplyRequest request)
    {
        Supply.OrderProduct(request);
    }

    public void Build(List<SupplyRequest> requests)
    {
        requests.ForEach(request => Supply.OrderProduct(request));
    }
}