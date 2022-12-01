namespace Banks.Exceptions;

public class BankAccountBuilderException : Exception
{
    private BankAccountBuilderException(string message = "")
        : base(message) { }

    public static BankAccountBuilderException WrondExpirationDate(string message = "") => new BankAccountBuilderException(message);
    public static BankAccountBuilderException WrondAccountId(string message = "") => new BankAccountBuilderException(message);
    public static BankAccountBuilderException WrondCreditRate(string message = "") => new BankAccountBuilderException(message);
}