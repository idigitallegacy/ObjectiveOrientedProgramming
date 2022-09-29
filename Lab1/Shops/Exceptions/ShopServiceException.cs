namespace Shops.Exceptions;

public class ShopServiceException : Exception
{
    public ShopServiceException(string message)
        : base(message) { }
}