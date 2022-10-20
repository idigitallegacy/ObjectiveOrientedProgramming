using Isu.Extra.Models;

namespace Isu.Extra.Composites;

public interface IScheduler
{
    public void AddLesson(LessonDto lessonDto);

    public void RemoveLesson(LessonDto lessonDto);

    public LessonDto? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
}