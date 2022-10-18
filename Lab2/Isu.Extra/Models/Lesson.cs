using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Wrappers;
using Isu.Models;
using Stream = System.IO.Stream;

namespace Isu.Extra.Models;

public class Lesson : IReadOnlyLesson, IEquatable<Lesson>
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
            throw LessonException.NoAssignee();
        _dayOfWeek = dayOfWeek;
        _startTime = startTime;
        _endTime = endTime;
        _teacher = teacher;
        _audience = audience;
        _associatedStream = associatedStream;
        _associatedGroup = associatedGroup;
    }

    public Lesson(IReadOnlyLesson copiedLesson)
    {
        _dayOfWeek = copiedLesson.DayOfWeek;
        _startTime = copiedLesson.StartTime;
        _endTime = copiedLesson.EndTime;
        _teacher = new Teacher(copiedLesson.Teacher);
        _audience = new Audience(copiedLesson.Audience);
        if (copiedLesson.AssociatedStream is not null)
            _associatedStream = new StudyStream(copiedLesson.AssociatedStream);
        if (copiedLesson.AssociatedGroup is not null)
            _associatedGroup = new ExtendedGroup(copiedLesson.AssociatedGroup);
    }

    public DayOfWeek DayOfWeek => _dayOfWeek;
    public Audience Audience => _audience;
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
        return HashCode.Combine((int)_dayOfWeek, _startTime, _endTime, _teacher, _audience, _associatedStream, _associatedGroup);
    }

    public bool Equals(Lesson? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _dayOfWeek == other._dayOfWeek && _startTime.Equals(other._startTime) && _endTime.Equals(other._endTime) && _teacher.Equals(other._teacher) && _audience.Equals(other._audience) && Equals(_associatedStream, other._associatedStream) && Equals(_associatedGroup, other._associatedGroup);
    }
}