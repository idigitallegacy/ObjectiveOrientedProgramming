using Isu.Extra.Entities;
using Isu.Extra.Extensions;
using Isu.Extra.Wrappers;
using Isu.Models;
using Stream = System.IO.Stream;

namespace Isu.Extra.Models;

public class Lesson : IReadOnlyLesson
{
    private DayOfWeek _dayOfWeek;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private Teacher _teacher;
    private Audience _audience;
    private StudyStream? _associatedStream;
    private ExtendedGroup? _associatedGroup;

    public Lesson(
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Teacher teacher,
        Audience audience,
        StudyStream? associatedStream = null,
        ExtendedGroup? associatedGroup = null)
    {
        if (associatedStream is null && associatedGroup is null)
            throw new Exception(); // TODO
        _dayOfWeek = dayOfWeek;
        _startTime = startTime;
        _endTime = endTime;
        _teacher = teacher;
        _audience = audience;
        _associatedStream = associatedStream;
        _associatedGroup = associatedGroup;
    }

    public DayOfWeek DayOfWeek => _dayOfWeek;
    public TimeSpan StartTime => _startTime;
    public TimeSpan EndTime => _endTime;
    public Teacher Teacher => _teacher;
    public StudyStream? AssociatedStream => _associatedStream;
    public ExtendedGroup? AssociatedGroup => _associatedGroup;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Lesson)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_startTime, _endTime, _teacher, _associatedStream, _associatedGroup, (int)_dayOfWeek);
    }
}