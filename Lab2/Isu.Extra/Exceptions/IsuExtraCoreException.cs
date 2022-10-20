using Isu.Entities;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Exceptions;

public class IsuExtraCoreException : Exception
{
    private IsuExtraCoreException(string message = "")
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

    public static IsuExtraCoreException CourseNotFound()
    {
        return new IsuExtraCoreException("Unable to find OGNP course.");
    }

    public static IsuExtraCoreException GroupNotFound(object group)
    {
        if (group is Group)
            return new IsuExtraCoreException($"Unable to find group {((Group)group).GroupName.Name}");
        if (group is IExtendedGroupDto)
            return new IsuExtraCoreException($"Unable to find group {((IExtendedGroupDto)group).GroupName.Name}");
        return new IsuExtraCoreException("Unable to find group.");
    }

    public static IsuExtraCoreException TeacherNotFound(ITeacherDto? teacher)
    {
        if (teacher is null)
            return new IsuExtraCoreException("Unable to find teacher: null reference.");
        return new IsuExtraCoreException($"Unable to find teacher {teacher.Name}.");
    }

    public static IsuExtraCoreException AudienceNotFound()
    {
        return new IsuExtraCoreException("Provided audience could not be found");
    }
}