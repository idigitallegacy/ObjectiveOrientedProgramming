using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Extensions;

public static class Typecast
{
    public static Audience ToAudience(this IReadOnlyAudience audience)
    {
        return new Audience(audience);
    }

    public static ExtendedGroup ToExtendedGroup(this IReadOnlyExtendedGroup group)
    {
        return new ExtendedGroup(group);
    }

    public static ExtendedStudent ToExtendedStudent(this IReadOnlyExtendedStudent student)
    {
        return new ExtendedStudent(student);
    }

    public static Lesson ToLesson(this IReadOnlyLesson lesson)
    {
        return new Lesson(lesson);
    }

    public static OgnpCourse ToOgnpCourse(this IReadOnlyOgnpCourse course)
    {
        return new OgnpCourse(course);
    }

    public static Schedule ToSchedule(this IReadOnlySchedule schedule)
    {
        return new Schedule(schedule);
    }

    public static StudyStream ToStream(this IReadOnlyStudyStream stream)
    {
        return new StudyStream(stream);
    }

    public static Teacher ToTeacher(this IReadOnlyTeacher teacher)
    {
        return new Teacher(teacher);
    }
}