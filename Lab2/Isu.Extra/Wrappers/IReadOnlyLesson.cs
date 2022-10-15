using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Wrappers;

public interface IReadOnlyLesson
{
    public DayOfWeek DayOfWeek { get; }
    public Time StartTime { get; }
    public Time EndTime { get; }
    public Teacher Teacher { get; }
    public StudyStream? AssociatedStream { get; }
    public ExtendedGroup? AssociatedGroup { get; }
}