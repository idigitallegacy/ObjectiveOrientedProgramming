using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Wrappers;

public interface ILessonDto
{
    public DayOfWeek DayOfWeek { get; }
    public AudienceDto AudienceDto { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public TeacherDto TeacherDto { get; }
    public StudyStreamDto? AssociatedStream { get; }
    public ExtendedGroupDto? AssociatedGroup { get; }
}