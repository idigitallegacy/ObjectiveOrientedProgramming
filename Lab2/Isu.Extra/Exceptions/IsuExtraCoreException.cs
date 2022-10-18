namespace Isu.Extra.Exceptions;

public class IsuExtraCoreException : Exception
{
    public IsuExtraCoreException(string message = "")
        : base(message) { }

    public static IsuExtraCoreException WrongFacultyId()
    {
        return new IsuExtraCoreException("Faculty id mismatch.");
    }

    public static IsuExtraCoreException StreamIsNotSet()
    {
        return new IsuExtraCoreException("Stream is not set.");
    }

    public static IsuExtraCoreException StudentHasAllOgnp()
    {
        return new IsuExtraCoreException("Unable to schedule student with new OGNP course: he's already have enough.");
    }
}