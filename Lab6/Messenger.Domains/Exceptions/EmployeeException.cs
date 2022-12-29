namespace Messenger.Domains.Exceptions;

public class EmployeeException : Exception
{
    private EmployeeException(string message = "") : base(message) { }
    public static EmployeeException RecursiveOrdinatory(string message = "") => new EmployeeException(message);
}