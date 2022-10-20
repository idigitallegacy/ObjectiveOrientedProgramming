using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public interface IScheduleDto
{
    public IReadOnlyCollection<LessonDto> Lessons { get; }

    public LessonDto? FindLesson(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);

    public bool TimeIsScheduled(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
}