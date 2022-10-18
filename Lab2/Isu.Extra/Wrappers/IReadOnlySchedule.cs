using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlySchedule
{
    public IReadOnlyCollection<Lesson> Lessons { get; }

    public Lesson? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);

    public bool TimeIsScheduled(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
}