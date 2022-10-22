using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Wrappers;
using Isu.Models;
using Stream = System.IO.Stream;

namespace Isu.Extra.Models;

public class Lesson : IEquatable<Lesson>
{
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
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Teacher = teacher;
        AudienceDto = new AudienceDto(audience);
        AssociatedStream = associatedStream;
        AssociatedGroup = associatedGroup;
    }

    public Lesson(LessonDto copiedLessonDto)
    {
        DayOfWeek = copiedLessonDto.DayOfWeek;
        StartTime = copiedLessonDto.StartTime;
        EndTime = copiedLessonDto.EndTime;
        Teacher = new TeacherDto(copiedLessonDto.Teacher).ToTeacher();
        AudienceDto = new AudienceDto(copiedLessonDto.AudienceDto.ToAudience());
        if (copiedLessonDto.AssociatedStream is not null)
            AssociatedStream = new StudyStream(new StudyStreamDto(copiedLessonDto.AssociatedStream));
        if (copiedLessonDto.AssociatedGroup is not null)
            AssociatedGroup = new ExtendedGroup(new ExtendedGroupDto(copiedLessonDto.AssociatedGroup));
    }

    public DayOfWeek DayOfWeek { get; }
    public AudienceDto AudienceDto { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }

    public Teacher Teacher { get; }
    public StudyStream? AssociatedStream { get; }
    public ExtendedGroup? AssociatedGroup { get; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Lesson)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)DayOfWeek, StartTime, EndTime, Teacher, AudienceDto, AssociatedStream, AssociatedGroup);
    }

    public bool Equals(Lesson? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DayOfWeek == other.DayOfWeek && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime) && Teacher.Equals(other.Teacher) && AudienceDto.Equals(other.AudienceDto) && Equals(AssociatedStream, other.AssociatedStream) && Equals(AssociatedGroup, other.AssociatedGroup);
    }
}