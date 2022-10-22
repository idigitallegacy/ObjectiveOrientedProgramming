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

    public static ExtendedGroupDto ToExtendedGroup(this IExtendedGroupDto groupDto)
    {
        return new ExtendedGroupDto(groupDto);
    }

    public static ExtendedStudentDto ToExtendedStudent(this IExtendedStudentDto studentDto)
    {
        return new ExtendedStudentDto(studentDto);
    }

    public static LessonDto ToLesson(this ILessonDto lessonDto)
    {
        return new LessonDto(lessonDto);
    }

    public static OgnpCourseDto ToOgnpCourse(this IOgnpCourseDto courseDto)
    {
        return new OgnpCourseDto(courseDto);
    }

    public static ScheduleDto ToSchedule(this IScheduleDto scheduleDto)
    {
        return new ScheduleDto(scheduleDto);
    }

    public static StudyStreamDto ToStream(this IStudyStreamDto streamDto)
    {
        return new StudyStreamDto(streamDto);
    }

    public static TeacherDto ToTeacher(this ITeacherDto teacherDto)
    {
        return new TeacherDto(teacherDto);
    }
}