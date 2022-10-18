using Isu.Entities;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class StudyStream : IScheduler, IReadOnlyStudyStream
{
    private Schedule _schedule;
    private ExtendedGroup _group;

    public StudyStream(GroupName streamName, int streamCapacity)
    {
        _schedule = new Schedule();
        _group = new ExtendedGroup(streamName, streamCapacity);
    }

    public IReadOnlySchedule Schedule => _schedule;
    public ExtendedGroup Group => _group;

    public void AddStudent(IReadOnlyExtendedStudent student)
    {
        ValidateStudent(student);
        _group.AddStudent(student);
    }

    public void RemoveStudent(IReadOnlyExtendedStudent student)
    {
        ExtendedStudent rwStudent = (ExtendedStudent)student;
        _group.RemoveStudent(student);
    }

    public void AddLesson(Lesson lesson)
    {
        _group.AddLesson(lesson);
        _schedule.AddLesson(lesson);
    }

    public void RemoveLesson(Lesson lesson)
    {
        _schedule.RemoveLesson(lesson);
    }

    public Lesson? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _schedule.FindLesson(dayOfWeek, startTime, endTime);
    }

    private void ValidateStudent(IReadOnlyExtendedStudent student)
    {
        bool studentGroupTimeIsScheduled =
            _schedule.Lessons.Any(lesson =>
            {
                return student.ExtendedGroup.Schedule
                    .TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime);
            });
        bool studentIsScheduledByOgnp =
            student.OgnpCourses.Any(course =>
            {
                bool? returnValue = course?.Streams
                    .Where(stream => stream.Group.Students.Contains(student))
                    .Any(stream =>
                    {
                        return _schedule.Lessons.Any(lesson =>
                            stream.Schedule.TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime));
                    });
                return returnValue is not null && returnValue == true;
            });
        if (studentGroupTimeIsScheduled || studentIsScheduledByOgnp)
            throw SchedulerException.TimeIsAlreadyScheduled();
    }
}