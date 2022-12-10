namespace Banks.Exceptions;

public class AddressBuilderException : Exception
{
    private AddressBuilderException(string message = "")
        : base(message) { }

    public static AddressBuilderException WrongCountry(string message = "") => new AddressBuilderException(message);
    public static AddressBuilderException WrongCity(string message = "") => new AddressBuilderException(message);
    public static AddressBuilderException WrongStreet(string message = "") => new AddressBuilderException(message);
}