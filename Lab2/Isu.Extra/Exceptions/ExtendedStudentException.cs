using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class ExtendedStudentException : Exception
{
    public ExtendedStudentException(string message = "")
        : base(message) { }

    public static ExtendedStudentException StudentHasAllOgnp()
    {
        return new ExtendedStudentException("Unable to schedule student with new OGNP course: he's already have enough.");
    }

    public static ExtendedStudentException UnableToFindOgnp()
    {
        return new ExtendedStudentException($"Unable to find course.");
    }
}