namespace Messenger.Domains.Exceptions;

public class ApplicationException : Exception
{
    private ApplicationException(string message = "") : base(message) { }
    public static ApplicationException OrdinateHasDirector(string message = "") => new ApplicationException(message);
    public static ApplicationException OrdinateIsNotDirector(string message = "") => new ApplicationException(message);
}