using Shops.Entities;

namespace Shops.Models;

public class Supply
{
    private List<Product> _products = new ();

    public Supply() { }

    private Supply(List<Product> products)
    {
        _products = new List<Product>(products);
    }

    public IReadOnlyCollection<Product> Products => _products;

    public void OrderProduct(SupplyRequest request)
    {
        _products.Add(new Product(request.Properties));
    }

    public object ShallowCopy()
    {
        return MemberwiseClone();
    }

    public Supply DeepCopy()
    {
        return new Supply(_products);
    }
}