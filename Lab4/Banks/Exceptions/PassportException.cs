namespace Banks.Exceptions;

public class PassportException : Exception
{
    private PassportException(string message = "")
        : base(message) { }
    public static PassportException WrongGivenBy(string message = "") => new PassportException(message);
    public static PassportException WrongDivisionCode(string message = "") => new PassportException(message);
}