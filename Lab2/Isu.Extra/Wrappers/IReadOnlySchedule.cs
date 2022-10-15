using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlySchedule
{
    public IReadOnlyCollection<Lesson> Lessons { get; }

    public Lesson? FindLesson(DayOfWeek dayOfWeek, Time startTime, Time endTime);

    public bool TimeIsScheduled(DayOfWeek dayOfWeek, Time startTime, Time endTime);
}