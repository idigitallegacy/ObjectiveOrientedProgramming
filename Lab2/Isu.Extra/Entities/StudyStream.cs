using Isu.Entities;
using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;
using Isu.Models;

namespace Isu.Extra.Entities;

public class StudyStream : IScheduler, IEquatable<StudyStream>
{
    private Schedule _schedule;
    private ExtendedGroup _group;

    public StudyStream(GroupName streamName, int streamCapacity)
    {
        _schedule = new Schedule();
        _group = new ExtendedGroup(streamName, streamCapacity);
    }

    public StudyStream(StudyStreamDto copiedStreamDto)
    {
        _schedule = new Schedule(copiedStreamDto.Schedule);
        _group = new ExtendedGroup(new ExtendedGroupDto(copiedStreamDto.Group));
    }

    public ScheduleDto Schedule => new ScheduleDto(_schedule);
    public ExtendedGroup Group => _group;

    public void AddStudent(ExtendedStudentDto studentDto)
    {
        ValidateStudent(studentDto);
        _group.AddStudent(studentDto);
    }

    public void RemoveStudent(ExtendedStudent student)
    {
        _group.RemoveStudent(student);
    }

    public void AddLesson(Lesson lesson)
    {
        _group.AddLesson(new LessonDto(lesson));
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
        return _group.GroupName.Equals(other._group.GroupName);
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

    private void ValidateStudent(ExtendedStudentDto studentDto)
    {
        bool studentGroupTimeIsScheduled =
            _schedule.Lessons.Any(lesson =>
            {
                return studentDto.ExtendedGroup.ScheduleDto.ToSchedule()
                    .TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime);
            });
        bool studentIsScheduledByOgnp =
            studentDto.OgnpCourses.Any(course =>
            {
                bool? returnValue = course?.Streams
                    .Where(stream => stream.Group.Students.Contains(studentDto.ToExtendedStudent()))
                    .Any(stream =>
                    {
                        return _schedule.Lessons.Any(lesson =>
                            stream.Schedule.ToSchedule()
                                .TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime));
                    });
                return returnValue is not null && returnValue == true;
            });
        if (studentGroupTimeIsScheduled || studentIsScheduledByOgnp)
            throw SchedulerException.TimeIsAlreadyScheduled();
    }
}