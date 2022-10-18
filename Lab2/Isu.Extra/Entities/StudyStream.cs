using Isu.Entities;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class StudyStream : IScheduler, IReadOnlyStudyStream, IEquatable<StudyStream>
{
    private Schedule _schedule;
    private ExtendedGroup _group;

    public StudyStream(GroupName streamName, int streamCapacity)
    {
        _schedule = new Schedule();
        _group = new ExtendedGroup(streamName, streamCapacity);
    }

    public StudyStream(IReadOnlyStudyStream copiedStream)
    {
        _schedule = new Schedule(copiedStream.Schedule);
        _group = new ExtendedGroup(copiedStream.Group);
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

    public bool Equals(StudyStream? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _schedule.Equals(other._schedule) && _group.Equals(other._group);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StudyStream)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_schedule, _group);
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