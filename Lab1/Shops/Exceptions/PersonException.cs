namespace Shops.Exceptions;

public class PersonException : Exception
{
    public PersonException(string message = "")
        : base(message)
    {
    }
}