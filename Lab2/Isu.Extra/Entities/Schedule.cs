using Isu.Extra.Composites;
using Isu.Extra.Models;
using Isu.Extra.Wrappers;

namespace Isu.Extra.Entities;

public class Schedule : Scheduler, IReadOnlySchedule
{
    private List<Lesson> _lessons = new ();

    IReadOnlyCollection<Lesson> IReadOnlySchedule.Lessons => _lessons;

    public IReadOnlyCollection<Lesson> Lessons => _lessons;

    public override void AddLesson(Lesson lesson)
    {
        if (TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
        _lessons.Add(lesson);
    }

    public override void RemoveLesson(Lesson lesson)
    {
        if (TimeIsScheduled(lesson.DayOfWeek, lesson.StartTime, lesson.EndTime))
            throw new Exception(); // TODO
        _lessons.Add(lesson);
    }

    public override Lesson? FindLesson(DayOfWeek dayOfWeek, Time startTime, Time endTime)
    {
        return _lessons.FirstOrDefault(lesson =>
        {
            return lesson.DayOfWeek == dayOfWeek &&
                   lesson.StartTime == startTime &&
                   lesson.EndTime == endTime;
        });
    }

    public bool TimeIsScheduled(DayOfWeek dayOfWeek, Time startTime, Time endTime)
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
}