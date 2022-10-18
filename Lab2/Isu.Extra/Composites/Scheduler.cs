using Isu.Extra.Models;

namespace Isu.Extra.Composites;

public interface IScheduler
{
    public void AddLesson(Lesson lesson);

    public void RemoveLesson(Lesson lesson);

    public Lesson? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
}