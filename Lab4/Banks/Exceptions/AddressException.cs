namespace Banks.Exceptions;

public class AddressException : Exception
{
    private AddressException(string message = "")
        : base(message) { }

    public static AddressException WrongCountry(string message = "") => new AddressException(message);
    public static AddressException WrongCity(string message = "") => new AddressException(message);
    public static AddressException WrongStreet(string message = "") => new AddressException(message);
}