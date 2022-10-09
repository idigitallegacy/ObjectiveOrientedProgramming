namespace Shops.Exceptions;

public class ProductPropertyException : Exception
{
    public ProductPropertyException(string message = "")
        : base(message)
    {
    }
}