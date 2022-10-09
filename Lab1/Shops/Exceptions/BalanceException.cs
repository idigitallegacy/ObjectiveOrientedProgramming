namespace Shops.Exceptions;

public class BalanceException : Exception
{
    public BalanceException(string message = "")
        : base(message)
    {
    }
}