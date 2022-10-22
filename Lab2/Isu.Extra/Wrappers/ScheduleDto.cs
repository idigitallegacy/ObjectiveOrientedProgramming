using Isu.Extra.Composites;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Wrappers;

public class ScheduleDto
{
    public ScheduleDto(Schedule schedule)
    {
        Lessons = schedule.Lessons;
    }

    public IReadOnlyCollection<Lesson> Lessons { get; }
}