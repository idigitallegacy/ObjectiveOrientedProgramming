using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Models;

public class Audience : IScheduler, IEquatable<Audience>
{
    private Schedule _schedule;
    private int _number;

    public Audience(int number)
    {
        if (number < 0)
            throw SchedulerException.WrongAudienceNumber(number);
        _number = number;
        _schedule = new Schedule();
    }

    public Audience(AudienceDto copiedAudience)
    {
        _schedule = new Schedule(copiedAudience.ScheduleDto);
        _number = copiedAudience.Number;
    }

    public ScheduleDto Schedule => new ScheduleDto(_schedule);
    public int Number => _number;

    public void AddLesson(Lesson lesson)
    {
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

    public bool Equals(Audience? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _number == other._number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Audience)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_schedule, _number);
    }
}