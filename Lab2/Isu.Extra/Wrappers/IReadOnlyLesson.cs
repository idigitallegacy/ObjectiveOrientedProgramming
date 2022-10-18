using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyLesson
{
    public DayOfWeek DayOfWeek { get; }
    public Audience Audience { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public Teacher Teacher { get; }
    public StudyStream? AssociatedStream { get; }
    public ExtendedGroup? AssociatedGroup { get; }
}