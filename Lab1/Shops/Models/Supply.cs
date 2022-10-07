using Shops.Entities;

namespace Shops.Models;

public class Supply
{
    private List<Product> _products = new ();

    public IReadOnlyCollection<Product> Products => _products;

    public void OrderProduct(SupplyRequest request)
    {
        _products.Add(new Product(request.Properties));
    }
}