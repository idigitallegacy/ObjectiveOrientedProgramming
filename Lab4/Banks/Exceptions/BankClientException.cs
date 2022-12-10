namespace Banks.Exceptions;

public class BankClientException : Exception
{
    private BankClientException(string message = "")
        : base(message) { }

    public static BankClientException WrongName(string message = "") => new BankClientException(message);
    public static BankClientException WrongSurname(string message = "") => new BankClientException(message);
}