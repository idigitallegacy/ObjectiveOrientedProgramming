using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Wrappers;

public class LessonDto
{
    public LessonDto(Lesson lesson)
    {
        DayOfWeek = lesson.DayOfWeek;
        AudienceDto = lesson.AudienceDto;
        StartTime = lesson.StartTime;
        EndTime = lesson.EndTime;
        Teacher = lesson.Teacher;
        AssociatedGroup = lesson.AssociatedGroup;
        AssociatedStream = lesson.AssociatedStream;
    }

    public DayOfWeek DayOfWeek { get; }
    public AudienceDto AudienceDto { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public Teacher Teacher { get; }
    public StudyStream? AssociatedStream { get; }
    public ExtendedGroup? AssociatedGroup { get; }

    public Lesson ToLesson()
    {
        return new Lesson(this);
    }
}