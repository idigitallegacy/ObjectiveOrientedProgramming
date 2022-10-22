using Isu.Extra.Entities;
using Isu.Models;

namespace Isu.Extra.Exceptions;

public class OgnpCourseException : Exception
{
    private OgnpCourseException(string message = "")
        : base(message) { }

    public static OgnpCourseException TooFewTeachers()
    {
        return new OgnpCourseException("Too few teachers to construct course.");
    }

    public static OgnpCourseException TooFewStreams()
    {
        return new OgnpCourseException("Too few streams to construct course.");
    }

    public static OgnpCourseException StreamNotFound(GroupName steamName)
    {
        return new OgnpCourseException($"Stream named {steamName.Name} not found.");
    }

    public static OgnpCourseException TeacherNotFound(Teacher teacher)
    {
        return new OgnpCourseException($"Teacher {teacher.Name} not found.");
    }

    public static OgnpCourseException WrongFacultyId()
    {
        return new OgnpCourseException("Faculty id mismatch.");
    }
}