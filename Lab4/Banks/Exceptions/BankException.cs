namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string message = "")
        : base(message) { }

    public static BankException WrongMoneyAmount(string message = "") => new BankException(message);
    public static BankException WithdrawLimitExceed(string message = "") => new BankException(message);
    public static BankException NotPermittedOperation(string message = "") => new BankException(message);
    public static BankException BadTransactionId(string message = "") => new BankException(message);
    public static BankException WrongClient(string message = "") => new BankException(message);
}