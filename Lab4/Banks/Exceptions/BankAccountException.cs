namespace Banks.Exceptions;

public class BankAccountException : Exception
{
    private BankAccountException(string message = "")
        : base(message) { }

    public static BankAccountException WrongMoneyAmount(string message = "") => new BankAccountException(message);
    public static BankAccountException WithdrawLimitExceed(string message = "") => new BankAccountException(message);
    public static BankAccountException NotEnoughMoney(string message = "") => new BankAccountException(message);
    public static BankAccountException OperationNotPermitted(string message = "") => new BankAccountException(message);
}