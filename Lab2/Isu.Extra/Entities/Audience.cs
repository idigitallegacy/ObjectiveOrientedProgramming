using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Models;

public class Audience : IScheduler, IReadOnlyAudience
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

    public Audience(IReadOnlyAudience copiedAudience)
    {
        _schedule = new Schedule(copiedAudience.Schedule);
        _number = copiedAudience.Number;
    }

    public IReadOnlySchedule Schedule => _schedule;
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
}