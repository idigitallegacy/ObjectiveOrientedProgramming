using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Models;

public class Audience : Scheduler, IReadOnlyAudience
{
    private Schedule _schedule;
    private int _number;

    public Audience(int number)
    {
        if (number < 0)
            throw new Exception(); // TODO
        _number = number;
        _schedule = new Schedule();
    }

    public IReadOnlySchedule Schedule => _schedule;
    public int Number => _number;

    public override void AddLesson(Lesson lesson)
    {
        _schedule.AddLesson(lesson);
    }

    public override void RemoveLesson(Lesson lesson)
    {
        _schedule.RemoveLesson(lesson);
    }

    public override Lesson? FindLesson(DayOfWeek dayOfWeek, Time startTime, Time endTime)
    {
        return _schedule.FindLesson(dayOfWeek, startTime, endTime);
    }
}