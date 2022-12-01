namespace Banks.Exceptions;

public class TimeFlowException : Exception
{
    private TimeFlowException(string message = "")
        : base(message) { }

    public static TimeFlowException WrongNewDate(string message = "") => new TimeFlowException(message);
}