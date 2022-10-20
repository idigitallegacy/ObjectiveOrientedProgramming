using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Extensions;
using Isu.Extra.Wrappers;
using Isu.Models;
using Stream = System.IO.Stream;

namespace Isu.Extra.Models;

public class LessonDto : ILessonDto, IEquatable<LessonDto>
{
    public LessonDto(
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        TeacherDto teacherDto,
        AudienceDto audienceDto,
        StudyStreamDto? associatedStream = null,
        ExtendedGroupDto? associatedGroup = null)
    {
        if (associatedStream is null && associatedGroup is null)
            throw LessonException.NoAssignee();
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        TeacherDto = teacherDto;
        AudienceDto = audienceDto;
        AssociatedStream = associatedStream;
        AssociatedGroup = associatedGroup;
    }

    public LessonDto(ILessonDto copiedLessonDto)
    {
        DayOfWeek = copiedLessonDto.DayOfWeek;
        StartTime = copiedLessonDto.StartTime;
        EndTime = copiedLessonDto.EndTime;
        TeacherDto = new TeacherDto(copiedLessonDto.TeacherDto);
        AudienceDto = new AudienceDto(copiedLessonDto.AudienceDto);
        if (copiedLessonDto.AssociatedStream is not null)
            AssociatedStream = new StudyStreamDto(copiedLessonDto.AssociatedStream);
        if (copiedLessonDto.AssociatedGroup is not null)
            AssociatedGroup = new ExtendedGroupDto(copiedLessonDto.AssociatedGroup);
    }

    public DayOfWeek DayOfWeek { get; }
    public AudienceDto AudienceDto { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }

    public TeacherDto TeacherDto { get; }
    public StudyStreamDto? AssociatedStream { get; }
    public ExtendedGroupDto? AssociatedGroup { get; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LessonDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)DayOfWeek, StartTime, EndTime, TeacherDto, AudienceDto, AssociatedStream, AssociatedGroup);
    }

    public bool Equals(LessonDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DayOfWeek == other.DayOfWeek && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime) && TeacherDto.Equals(other.TeacherDto) && AudienceDto.Equals(other.AudienceDto) && Equals(AssociatedStream, other.AssociatedStream) && Equals(AssociatedGroup, other.AssociatedGroup);
    }
}