namespace Banks.Exceptions;

public class BankClientBuilderException : Exception
{
    private BankClientBuilderException(string message = "")
        : base(message) { }

    public static BankClientBuilderException WrongName(string message = "") => new BankClientBuilderException(message);
    public static BankClientBuilderException WrongSurname(string message = "") => new BankClientBuilderException(message);
    public static BankClientBuilderException WrongId(string message = "") => new BankClientBuilderException(message);
}