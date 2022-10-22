using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Extensions;

public static class Typecast
{
    public static Audience ToAudience(this AudienceDto audienceDto)
    {
        return new Audience(audienceDto);
    }

    public static ExtendedGroup ToExtendedGroup(this ExtendedGroupDto groupDto)
    {
        return new ExtendedGroup(groupDto);
    }

    public static ExtendedStudent ToExtendedStudent(this ExtendedStudentDto studentDto)
    {
        return new ExtendedStudent(studentDto);
    }

    public static Lesson ToLesson(this LessonDto lessonDto)
    {
        return new Lesson(lessonDto);
    }

    public static OgnpCourse ToOgnpCourse(this OgnpCourseDto courseDto)
    {
        return new OgnpCourse(courseDto);
    }

    public static Schedule ToSchedule(this ScheduleDto scheduleDto)
    {
        return new Schedule(scheduleDto);
    }

    public static StudyStream ToStream(this StudyStreamDto streamDto)
    {
        return new StudyStream(streamDto);
    }

    public static Teacher ToTeacher(this TeacherDto teacherDto)
    {
        return new Teacher(teacherDto);
    }
}