using Isu.Extra.Models;

namespace Isu.Extra.Composites;

public abstract class Scheduler
{
    public abstract void AddLesson(Lesson lesson);

    public abstract void RemoveLesson(Lesson lesson);

    public abstract Lesson? FindLesson(DayOfWeek dayOfWeek, Time startTime, Time endTime);
}