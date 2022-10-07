namespace Shops.Exceptions;

public class RequestException : Exception
{
    public RequestException(string message = "")
        : base(message)
    {
    }
}