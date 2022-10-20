using Isu.Extra.Composites;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class ScheduleDto : IScheduler, IScheduleDto, IEquatable<ScheduleDto>
{
    private List<LessonDto> _lessons = new ();

    public ScheduleDto() { }
    public ScheduleDto(IScheduleDto copiedScheduleDto)
    {
        _lessons = new List<LessonDto>(copiedScheduleDto.Lessons);
    }

    IReadOnlyCollection<LessonDto> IScheduleDto.Lessons => _lessons.AsReadOnly();

    public IReadOnlyCollection<LessonDto> Lessons => _lessons;

    public void AddLesson(LessonDto lessonDto)
    {
        if (TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime))
            throw SchedulerException.TimeIsAlreadyScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime);
        _lessons.Add(lessonDto);
    }

    public void RemoveLesson(LessonDto lessonDto)
    {
        if (!TimeIsScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime))
            throw SchedulerException.TimeIsNotScheduled(lessonDto.DayOfWeek, lessonDto.StartTime, lessonDto.EndTime);
        _lessons.Add(lessonDto);
    }

    public LessonDto? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return _lessons.FirstOrDefault(lesson =>
        {
            return lesson.DayOfWeek == dayOfWeek &&
                   lesson.StartTime == startTime &&
                   lesson.EndTime == endTime;
        });
    }

    public bool TimeIsScheduled(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return Lessons
            .Select(lesson => new
            {
                dayOfWeek = lesson.DayOfWeek, startTime = lesson.StartTime, endTime = lesson.EndTime,
            })
            .Where(lesson => lesson.dayOfWeek == dayOfWeek)
            .Any(scheduledPeriod =>
            {
                return (scheduledPeriod.startTime > startTime && scheduledPeriod.startTime < endTime) ||
                       (scheduledPeriod.endTime > startTime && scheduledPeriod.endTime < endTime);
            });
    }

    public bool Equals(ScheduleDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _lessons.Equals(other._lessons);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ScheduleDto)obj);
    }

    public override int GetHashCode()
    {
        return _lessons.GetHashCode();
    }
}