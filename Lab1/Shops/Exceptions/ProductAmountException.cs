namespace Shops.Exceptions;

public class ProductAmountException : Exception
{
    public ProductAmountException(string message = "")
        : base(message)
    {
    }
}