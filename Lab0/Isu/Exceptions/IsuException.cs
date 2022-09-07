namespace Isu.Exceptions;

public class IsuException : Exception
{
    public IsuException()
        : base("Unhandled error")
    {
        Error = "Unhandled error";
    }

    public IsuException(string message)
        : base(message)
    {
        Error = message;
    }

    public string Error { get; }
}